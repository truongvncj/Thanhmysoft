using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleManagerApi.Data;
using SaleManagerApi.Models;

namespace SaleManagerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DangTaisController : ControllerBase
{
    private readonly AppDbContext _context;

    public DangTaisController(AppDbContext context)
    {
        _context = context;
    }

    // Lấy trạng thái trong ngày của 1 thẻ
    [HttpGet("state")]
    public async Task<IActionResult> GetState([FromQuery] string sothe)
    {
        if (string.IsNullOrEmpty(sothe))
        {
            return BadRequest(new { message = "Số thẻ không hợp lệ" });
        }

        sothe = sothe.Trim();
        var today = DateTime.UtcNow.AddHours(7).Date;

        var dangTai = await _context.DangTais
            .Where(d => d.Sothe == sothe && d.NgayDangTai.Date == today)
            .FirstOrDefaultAsync();

        var hasPreTrip = await _context.DailyPreTripChecklists
            .AnyAsync(p => p.Sothe == sothe && p.Date.Date == today);

        if (dangTai == null)
        {
            return Ok(new { sothe, daDangTai = hasPreTrip, daVeSinh = false });
        }

        return Ok(new { sothe, daDangTai = hasPreTrip, daVeSinh = dangTai.DaVeSinh });
    }

    [HttpPost("dangtai")]
    public async Task<IActionResult> DangTai([FromBody] DangTaiRequest request)
    {
        var sothe = request.Sothe?.Trim();
        if (string.IsNullOrEmpty(sothe)) return BadRequest();

        var today = DateTime.UtcNow.AddHours(7).Date;

        var activeTrip = await _context.Danhsachxetrongkhos
            .FirstOrDefaultAsync(x => x.Sothe == sothe && x.TrangThai < 3);

        if (activeTrip != null)
        {
            return Conflict(new { message = "Bạn đang có một chuyến xe chưa hoàn thành! Vui lòng hoàn tất chuyến hiện tại trước khi đăng ký chuyến mới." });
        }

        var lastDangTai = await _context.DangTais
            .Where(d => d.Sothe == sothe && d.NgayDangTai.Date == today)
            .OrderByDescending(d => d.NgayDangTai)
            .FirstOrDefaultAsync();

        var lastVeSinh = await _context.DailyKiemtravesinhxes
            .Where(v => v.Sothe == sothe && v.NgayKiemTra.Date == today)
            .OrderByDescending(v => v.NgayKiemTra)
            .FirstOrDefaultAsync();

        if (lastVeSinh == null || (lastDangTai != null && lastVeSinh.NgayKiemTra < lastDangTai.NgayDangTai))
        {
            return Conflict(new { message = "Bạn cần thực hiện kiểm tra vệ sinh xe mới cho chuyến đi này!" });
        }

        var driver = await _context.Thenhathaus.FirstOrDefaultAsync(t => t.Sothe.Trim() == sothe);
        string tenNvt = driver != null ? driver.TenNvt : "";

        var dangTai = new DangTai
        {
            Id = Guid.NewGuid(),
            Sothe = sothe,
            NgayDangTai = DateTime.UtcNow.AddHours(7),
            DaDangTai = true,
            DaVeSinh = true,
            TenNvt = tenNvt,
            KhohangId = request.KhohangId,
            LyDo = request.LyDo,
            BienSo = request.BienSo,
            TrangThai = 0 // Đã đăng ký
        };
        _context.DangTais.Add(dangTai);

        await SyncDanhsachxetrongkho(dangTai, sothe, driver);

        await _context.SaveChangesAsync();
        return Ok(new { message = "Đăng tài thành công" });
    }

    public class VesinhSubmitRequest
    {
        public string Sothe { get; set; } = string.Empty;
        public List<VesinhAnswer> Answers { get; set; } = new List<VesinhAnswer>();
    }

    public class VesinhAnswer
    {
        public int ParamId { get; set; }
        public bool KetQua { get; set; }
        public string? GhiChu { get; set; }
    }

    [HttpPost("vesinh")]
    public async Task<IActionResult> VeSinh([FromBody] VesinhSubmitRequest request)
    {
        var sothe = request.Sothe?.Trim();
        if (string.IsNullOrEmpty(sothe)) return BadRequest();

        var today = DateTime.UtcNow.AddHours(7).Date;


        var driver = await _context.Thenhathaus.FirstOrDefaultAsync(t => t.Sothe.Trim() == sothe);
        if (driver == null) return NotFound(new { message = "Không tìm thấy thẻ nhà thầu" });

        var activeParams = await _context.KiemtravesinhxeParams.Where(p => p.IsActive).ToListAsync();
        
        var details = new List<DailyKiemtravesinhxeDetail>();
        foreach(var param in activeParams)
        {
            var answer = request.Answers.FirstOrDefault(a => a.ParamId == param.Id);
            if (answer == null) 
            {
                return BadRequest(new { message = $"Thiếu câu trả lời cho mục: {param.NoiDung}" });
            }

            if (answer.KetQua != param.DapAnDat)
            {
                return BadRequest(new { message = "Không đạt tiêu chuẩn vệ sinh, vui lòng vệ sinh lại xe và kiểm tra lại." });
            }

            details.Add(new DailyKiemtravesinhxeDetail
            {
                ParamId = param.Id,
                KetQua = answer.KetQua,
                GhiChu = answer.GhiChu
            });
        }

        var lanVaoKho = await _context.DailyKiemtravesinhxes
            .Where(d => d.Sothe == sothe && d.NgayKiemTra.Date == today)
            .CountAsync() + 1;

        var hygieneCheck = new DailyKiemtravesinhxe
        {
            TenNhaThau = driver.TenNvt,
            TenLaiXe = driver.Ten,
            MaLx = driver.MaLx,
            MaNvt = driver.MaNvt,
            Sothe = driver.Sothe,
            NgayKiemTra = DateTime.UtcNow.AddHours(7),
            LanVaoKhoTrongNgay = lanVaoKho,
            Details = details
        };

        _context.DailyKiemtravesinhxes.Add(hygieneCheck);

        await _context.SaveChangesAsync();
        return Ok(new { message = "Xác nhận vệ sinh xe thành công" });
    }

    public class DangTaiRequest
    {
        public string Sothe { get; set; } = string.Empty;
        public int? KhohangId { get; set; }
        public string LyDo { get; set; } = string.Empty;
        public string BienSo { get; set; } = string.Empty;
    }

    public class ChecklistSubmitRequest
    {
        public string Sothe { get; set; } = string.Empty;
        public int? KhohangId { get; set; }
        public string LyDo { get; set; } = string.Empty;
        public string ChecklistData { get; set; } = string.Empty;
        public string BienSo { get; set; } = string.Empty;
    }

    [HttpPost("submit-checklist")]
    public async Task<IActionResult> SubmitChecklist([FromBody] ChecklistSubmitRequest request)
    {
        var sothe = request.Sothe?.Trim();
        if (string.IsNullOrEmpty(sothe)) return BadRequest();

        var today = DateTime.UtcNow.AddHours(7).Date;


        var hasPreTrip = await _context.DailyPreTripChecklists
            .AnyAsync(p => p.Sothe == sothe && p.Date.Date == today);

        if (hasPreTrip)
        {
            return Conflict(new { message = "Bạn đã hoàn thành Pre-Trip Checklist hôm nay rồi!" });
        }

        var driver = await _context.Thenhathaus.FirstOrDefaultAsync(t => t.Sothe.Trim() == sothe);
        if (driver == null) return NotFound(new { message = "Không tìm thấy thẻ nhà thầu" });

        var checklistRecord = new DailyPreTripChecklist
        {
            Id = Guid.NewGuid(),
            Date = DateTime.UtcNow.AddHours(7),
            MaLx = driver.MaLx,
            TenLx = driver.Ten,
            MaNvt = driver.MaNvt,
            TenNvt = driver.TenNvt,
            Sothe = driver.Sothe,
            ChecklistData = request.ChecklistData
        };
        _context.DailyPreTripChecklists.Add(checklistRecord);

        await _context.SaveChangesAsync();
        return Ok(new { message = "Hoàn thành Pre-Trip Checklist" });
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboardVehicles([FromQuery] int? khohangId)
    {
        var today = DateTime.UtcNow.AddHours(7).Date;
        var query = _context.DangTais
            .Where(d => d.NgayDangTai.Date == today && d.DaDangTai);

        if (khohangId.HasValue)
        {
            query = query.Where(d => d.KhohangId == khohangId.Value);
        }

        var dangtais = await query.ToListAsync();

        var result = new List<object>();

        foreach(var dt in dangtais)
        {
            var driver = await _context.Thenhathaus.FirstOrDefaultAsync(t => t.Sothe.Trim() == dt.Sothe);
            result.Add(new
            {
                dt.Id,
                dt.Sothe,
                dt.NgayDangTai,
                dt.DaDangTai,
                dt.DaVeSinh,
                dt.TenNvt,
                dt.KhohangId,
                dt.LyDo,
                dt.TrangThai,
                TenLaiXe = driver?.Ten ?? "",
                BienSo = driver?.MaLx ?? ""
            });
        }

        return Ok(result);
    }

    [HttpPost("update-status/{id}")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] int newStatus)
    {
        var dangTai = await _context.DangTais.FindAsync(id);
        if (dangTai == null) return NotFound(new { message = "Không tìm thấy xe." });

        if (newStatus < 0 || newStatus > 3) return BadRequest(new { message = "Trạng thái không hợp lệ." });

        dangTai.TrangThai = newStatus;
        _context.Entry(dangTai).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Cập nhật trạng thái thành công." });
    }

    private async Task SyncDanhsachxetrongkho(DangTai dangTai, string sothe, Thenhathau driver)
    {
        if (dangTai.DaDangTai)
        {
            var today = DateTime.UtcNow.AddHours(7).Date;
            var record = await _context.Danhsachxetrongkhos
                .FirstOrDefaultAsync(x => x.Sothe == sothe && x.GioDangKy == dangTai.NgayDangTai);
                
            var preTrip = await _context.DailyPreTripChecklists
                .Where(p => p.Sothe == sothe && p.Date.Date == today)
                .OrderByDescending(p => p.Date)
                .FirstOrDefaultAsync();
                
            var veSinh = await _context.DailyKiemtravesinhxes
                .Where(v => v.Sothe == sothe && v.NgayKiemTra.Date == today)
                .OrderByDescending(v => v.NgayKiemTra)
                .FirstOrDefaultAsync();

            if (record == null)
            {
                record = new Danhsachxetrongkho
                {
                    Id = Guid.NewGuid(),
                    Sothe = sothe,
                    TenLaiXe = driver.Ten,
                    MaNhanVienLaiXe = driver.MaLx,
                    BienSo = dangTai.BienSo,
                    TenNhaThauVanTai = driver.TenNvt,
                    GioDangKy = dangTai.NgayDangTai,
                    LyDoVaoKho = dangTai.LyDo,
                    KhohangId = dangTai.KhohangId,
                    Pre_Tripchecklist_Time = preTrip?.Date,
                    KiemTraVeSinh_Time = veSinh?.NgayKiemTra,
                    TrangThai = 0
                };
                _context.Danhsachxetrongkhos.Add(record);
            }
            else
            {
                if (record.Pre_Tripchecklist_Time == null && preTrip != null)
                    record.Pre_Tripchecklist_Time = preTrip.Date;
                    
                if (record.KiemTraVeSinh_Time == null && veSinh != null)
                    record.KiemTraVeSinh_Time = veSinh.NgayKiemTra;
                    
                record.KhohangId = dangTai.KhohangId;
                if (!string.IsNullOrEmpty(dangTai.LyDo)) record.LyDoVaoKho = dangTai.LyDo;
                
                _context.Entry(record).State = EntityState.Modified;
            }
        }
    }

    [HttpGet("history/pre-trip")]
    public async Task<IActionResult> GetPreTripHistory([FromQuery] string sothe)
    {
        if (string.IsNullOrEmpty(sothe)) return BadRequest(new { message = "Số thẻ không hợp lệ" });
        
        var history = await _context.DailyPreTripChecklists
            .Where(x => x.Sothe == sothe)
            .OrderByDescending(x => x.Date)
            .Take(50)
            .ToListAsync();
            
        return Ok(history);
    }

    [HttpGet("history/vesinh")]
    public async Task<IActionResult> GetVeSinhHistory([FromQuery] string sothe)
    {
        if (string.IsNullOrEmpty(sothe)) return BadRequest(new { message = "Số thẻ không hợp lệ" });
        
        var history = await _context.DailyKiemtravesinhxes
            .Include(x => x.Details)
            .ThenInclude(d => d.Param)
            .Where(x => x.Sothe == sothe)
            .OrderByDescending(x => x.NgayKiemTra)
            .Take(50)
            .ToListAsync();
            
        return Ok(history);
    }

    public class ChungTuSubmitRequest
    {
        public string Sothe { get; set; } = string.Empty;
        public List<ChungTuItem> ChungTus { get; set; } = new List<ChungTuItem>();
    }

    public class ChungTuItem
    {
        public string LyDo { get; set; } = string.Empty;
        public string? SoTransferOut { get; set; }
        public string? SoSTO { get; set; }
        public string? SoShipment { get; set; }
    }

    [HttpPost("submit-chungtu")]
    public async Task<IActionResult> SubmitChungTu([FromBody] ChungTuSubmitRequest request)
    {
        var sothe = request.Sothe?.Trim();
        if (string.IsNullOrEmpty(sothe)) return BadRequest(new { message = "Số thẻ không hợp lệ." });

        var today = DateTime.UtcNow.AddHours(7).Date;
        var dangTai = await _context.DangTais
            .Where(d => d.Sothe == sothe && d.NgayDangTai.Date == today)
            .OrderByDescending(d => d.NgayDangTai)
            .FirstOrDefaultAsync();

        if (dangTai == null || !dangTai.DaDangTai || !dangTai.DaVeSinh)
        {
            return BadRequest(new { message = "Xe chưa hoàn thành Pre-Trip hoặc Kiểm tra vệ sinh." });
        }

        var driver = await _context.Thenhathaus.FirstOrDefaultAsync(t => t.Sothe.Trim() == sothe);
        if (driver == null) return NotFound(new { message = "Không tìm thấy thẻ nhà thầu" });

        foreach (var item in request.ChungTus)
        {
            var chungTu = new ChungTuVaoKho
            {
                Id = Guid.NewGuid(),
                Sothe = sothe,
                KhohangId = dangTai.KhohangId,
                NgayDangKy = DateTime.UtcNow.AddHours(7),
                LyDo = item.LyDo,
                SoTransferOut = item.SoTransferOut,
                SoSTO = item.SoSTO,
                SoShipment = item.SoShipment
            };
            _context.ChungTuVaoKhos.Add(chungTu);
        }

        dangTai.DaDangKyChungTu = true;
        _context.Entry(dangTai).State = EntityState.Modified;

        await SyncDanhsachxetrongkho(dangTai, sothe, driver);

        await _context.SaveChangesAsync();
        return Ok(new { message = "Đăng ký chứng từ thành công." });
    }
}
