using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleManagerApi.Data;
using SaleManagerApi.Models;

namespace SaleManagerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DanhsachxetrongkhoController : ControllerBase
{
    private readonly AppDbContext _context;

    public DanhsachxetrongkhoController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard([FromQuery] int? khohangId)
    {
        var today = DateTime.UtcNow.AddHours(7).Date;
        var tomorrow = today.AddDays(1);
        var sevenDaysAgo = today.AddDays(-7);
        var query = _context.Danhsachxetrongkhos
            .Where(d => (d.GioDangKy >= today && d.GioDangKy < tomorrow) 
                     || d.TrangThai < 3 
                     || (d.XacNhanRaCong_Time != null && d.XacNhanRaCong_Time >= sevenDaysAgo && d.XacNhanRaCong_Time < tomorrow));

        if (khohangId.HasValue)
        {
            query = query.Where(d => d.KhohangId == khohangId.Value);
        }

        var list = await query.ToListAsync();

        var sothesCheckedToday = await _context.Danhsachxetrongkhos
            .Where(x => x.BaoVeKiemTra_PreTrip_Time != null && x.BaoVeKiemTra_PreTrip_Time.Value.Date == today)
            .Select(x => x.Sothe)
            .Distinct()
            .ToListAsync();

        var allChungTusToday = await _context.ChungTuVaoKhos
            .Where(c => c.NgayDangKy >= sevenDaysAgo && c.NgayDangKy < tomorrow)
            .ToListAsync();

        var allDanhsachToday = await _context.Danhsachxetrongkhos
            .Where(d => d.GioDangKy >= sevenDaysAgo && d.GioDangKy < tomorrow)
            .ToListAsync();

        var result = new List<object>();
        foreach (var dt in list)
        {
            var nextDt = allDanhsachToday.Where(d => d.Sothe == dt.Sothe && d.GioDangKy > dt.GioDangKy)
                             .OrderBy(d => d.GioDangKy)
                             .FirstOrDefault();

            var chungTus = allChungTusToday
                .Where(c => c.Sothe == dt.Sothe && 
                            c.NgayDangKy >= dt.GioDangKy && 
                            (nextDt == null || c.NgayDangKy < nextDt.GioDangKy))
                .ToList();

            var chungTuList = chungTus.Select(c => 
            {
                var docStr = c.LyDo == "Nhập hàng" ? (
                    !string.IsNullOrWhiteSpace(c.SoTransferOut) && !string.IsNullOrWhiteSpace(c.SoSTO) ? $"{c.SoTransferOut} - {c.SoSTO}" :
                    !string.IsNullOrWhiteSpace(c.SoTransferOut) ? c.SoTransferOut : c.SoSTO
                ) : c.SoShipment;
                
                if (string.IsNullOrWhiteSpace(docStr)) return null;
                return $"{c.LyDo}: {docStr}";
            }).Where(s => s != null).ToList();

            var stos = chungTus.Where(c => c.LyDo == "Nhập hàng")
                .Select(c => 
                    !string.IsNullOrWhiteSpace(c.SoTransferOut) && !string.IsNullOrWhiteSpace(c.SoSTO) ? $"{c.SoTransferOut} - {c.SoSTO}" :
                    !string.IsNullOrWhiteSpace(c.SoTransferOut) ? c.SoTransferOut : c.SoSTO)
                .Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

            var shipments = chungTus.Select(c => c.SoShipment).Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

            var traVeDetails = chungTus.Where(c => c.LyDo == "Nhập hàng trả về").Select(c => new
            {
                SoShipment = c.SoShipment,
                LyDoTraVe = c.LyDoTraVe,
                MaKhachHang = c.MaKhachHang
            }).ToList();

            var baoVeDaKiemTraPreTripHomNay = sothesCheckedToday.Contains(dt.Sothe);

            result.Add(new
            {
                dt.Id,
                dt.Sothe,
                dt.BienSo,
                dt.TenLaiXe,
                dt.TenNhaThauVanTai,
                dt.LyDoVaoKho,
                dt.TrangThai,
                dt.GioDangKy,
                dt.Pre_Tripchecklist_Time,
                dt.KiemTraVeSinh_Time,
                dt.XacNhanVaoCong_Time,
                dt.ThuKhoXacNhan_Time,
                dt.XuatKho_Time,
                dt.XacNhanRaCong_Time,
                dt.BaoVeKiemTra_Time,
                dt.BaoVeKiemTra_PreTrip_Time,
                dt.BaoVeKiemTraTrongKho_Time,
                dt.NhapKho_Time,
                dt.LyDoHuy,
                BaoVeDaKiemTraPreTripHomNay = baoVeDaKiemTraPreTripHomNay,
                ChungTus = chungTuList,
                STOs = stos,
                Shipments = shipments,
                TraVeDetails = traVeDetails,
                ThoiGianTrongSan = GetThoiGianTrongSan(dt)
            });
        }

        return Ok(result);
    }

    private string GetThoiGianTrongSan(Danhsachxetrongkho dt)
    {
        if (dt.XacNhanVaoCong_Time == null) return "00:00:00";
        
        var startTime = dt.XacNhanVaoCong_Time.Value;
        var endTime = dt.XacNhanRaCong_Time ?? DateTime.UtcNow.AddHours(7);
        var diff = endTime - startTime;
        
        return $"{(int)diff.TotalHours:00}:{diff.Minutes:00}:{diff.Seconds:00}";
    }

    [HttpPost("update-status/{id}")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] int newStatus)
    {
        var record = await _context.Danhsachxetrongkhos.FindAsync(id);
        if (record == null) return NotFound(new { message = "Không tìm thấy chuyến xe." });

        if (newStatus < 0 || newStatus > 3) return BadRequest(new { message = "Trạng thái không hợp lệ." });

        record.TrangThai = newStatus;
        
        // Cập nhật thời gian theo trạng thái
        var now = DateTime.UtcNow.AddHours(7);
        if (newStatus == 1) 
        {
            record.XacNhanVaoCong_Time = now;
        }
        else if (newStatus == 2)
        {
            record.ThuKhoXacNhan_Time = now;
        }
        else if (newStatus == 3)
        {
            record.XacNhanRaCong_Time = now;
        }

        _context.Entry(record).State = EntityState.Modified;
        
        // Optionally update the original DangTai table to keep them in sync if other parts of the system rely on it
        var today = DateTime.UtcNow.AddHours(7).Date;
        var dangTai = await _context.DangTais.FirstOrDefaultAsync(d => d.Sothe == record.Sothe && d.NgayDangTai.Date == today);
        if (dangTai != null)
        {
            dangTai.TrangThai = newStatus;
            _context.Entry(dangTai).State = EntityState.Modified;
        }

        await _context.SaveChangesAsync();

        return Ok(new { message = "Cập nhật trạng thái thành công." });
    }

    public class RejectDangKyRequest
    {
        public string Reason { get; set; } = string.Empty;
    }

    [HttpPost("reject-dangky/{id}")]
    public async Task<IActionResult> RejectDangKy(Guid id, [FromBody] RejectDangKyRequest request)
    {
        var record = await _context.Danhsachxetrongkhos.FindAsync(id);
        if (record == null) return NotFound(new { message = "Không tìm thấy chuyến xe." });

        if (record.TrangThai != 0) return BadRequest(new { message = "Chuyến xe không ở trạng thái Đã đăng ký." });

        record.TrangThai = 3; // Hoàn thành (nhưng bị hủy)
        record.LyDoHuy = request.Reason;
        record.XacNhanRaCong_Time = DateTime.UtcNow.AddHours(7);

        _context.Entry(record).State = EntityState.Modified;
        
        var today = DateTime.UtcNow.AddHours(7).Date;
        var tomorrow = today.AddDays(1);
        var dangTai = await _context.DangTais.FirstOrDefaultAsync(d => d.Sothe == record.Sothe && d.NgayDangTai >= today && d.NgayDangTai < tomorrow && d.TrangThai == 0);
        if (dangTai != null)
        {
            dangTai.TrangThai = 3;
            dangTai.LyDoHuy = request.Reason;
            _context.Entry(dangTai).State = EntityState.Modified;
        }

        await _context.SaveChangesAsync();

        return Ok(new { message = "Đã từ chối phiếu đăng ký thành công." });
    }

    [HttpPost("update-xuatkho/{id}")]
    public async Task<IActionResult> UpdateXuatKho(Guid id)
    {
        var record = await _context.Danhsachxetrongkhos.FindAsync(id);
        if (record == null) return NotFound(new { message = "Không tìm thấy chuyến xe." });

        if (record.TrangThai != 1) return BadRequest(new { message = "Chuyến xe không ở trạng thái Trong sân." });

        record.XuatKho_Time = DateTime.UtcNow.AddHours(7);
        _context.Entry(record).State = EntityState.Modified;
        
        await _context.SaveChangesAsync();

        return Ok(new { message = "Xác nhận xuất kho thành công." });
    }

    [HttpPost("update-nhapkho/{id}")]
    public async Task<IActionResult> UpdateNhapKho(Guid id)
    {
        var record = await _context.Danhsachxetrongkhos.FindAsync(id);
        if (record == null) return NotFound(new { message = "Không tìm thấy chuyến xe." });

        if (record.TrangThai != 1) return BadRequest(new { message = "Chuyến xe không ở trạng thái Trong sân." });

        record.NhapKho_Time = DateTime.UtcNow.AddHours(7);
        _context.Entry(record).State = EntityState.Modified;
        
        await _context.SaveChangesAsync();

        return Ok(new { message = "Xác nhận nhập kho thành công." });
    }

    [HttpPost("reject-nhapkho/{id}")]
    public async Task<IActionResult> RejectNhapKho(Guid id, [FromBody] RejectXuatKhoRequest request)
    {
        var record = await _context.Danhsachxetrongkhos.FindAsync(id);
        if (record == null) return NotFound(new { message = "Không tìm thấy chuyến xe." });

        if (record.TrangThai != 1) return BadRequest(new { message = "Chuyến xe không ở trạng thái Trong sân." });

        record.TrangThai = 2; // Cho ra cổng luôn
        record.NhapKho_Time = DateTime.UtcNow.AddHours(7);
        record.LyDoHuy = request.LyDoHuy;
        
        _context.Entry(record).State = EntityState.Modified;
        
        await _context.SaveChangesAsync();

        return Ok(new { message = "Đã hủy yêu cầu nhập kho và chuyển trạng thái ra cổng." });
    }

    public class RejectXuatKhoRequest
    {
        public string LyDoHuy { get; set; } = string.Empty;
    }

    [HttpPost("reject-xuatkho/{id}")]
    public async Task<IActionResult> RejectXuatKho(Guid id, [FromBody] RejectXuatKhoRequest request)
    {
        var record = await _context.Danhsachxetrongkhos.FindAsync(id);
        if (record == null) return NotFound(new { message = "Không tìm thấy chuyến xe." });

        if (record.TrangThai != 1) return BadRequest(new { message = "Chuyến xe không ở trạng thái Trong sân." });

        record.TrangThai = 2;
        record.ThuKhoXacNhan_Time = DateTime.UtcNow.AddHours(7);
        record.LyDoHuy = request.LyDoHuy;
        _context.Entry(record).State = EntityState.Modified;
        
        var today = DateTime.UtcNow.AddHours(7).Date;
        var dangTai = await _context.DangTais.FirstOrDefaultAsync(d => d.Sothe == record.Sothe && d.NgayDangTai.Date == today);
        if (dangTai != null)
        {
            dangTai.TrangThai = 2;
            dangTai.LyDoHuy = request.LyDoHuy;
            _context.Entry(dangTai).State = EntityState.Modified;
        }

        await _context.SaveChangesAsync();

        return Ok(new { message = "Hủy yêu cầu xuất kho thành công. Xe đã chuyển ra cổng." });
    }

    [HttpPost("update-changbuoc/{id}")]
    public async Task<IActionResult> UpdateChangBuoc(Guid id)
    {
        var record = await _context.Danhsachxetrongkhos.FindAsync(id);
        if (record == null) return NotFound(new { message = "Không tìm thấy chuyến xe." });

        if (record.TrangThai != 1) return BadRequest(new { message = "Chuyến xe không ở trạng thái Trong sân." });

        record.BaoVeKiemTraTrongKho_Time = DateTime.UtcNow.AddHours(7);
        _context.Entry(record).State = EntityState.Modified;
        
        await _context.SaveChangesAsync();

        return Ok(new { message = "Xác nhận chằng buộc thành công." });
    }

    public class VesinhAnswerDto
    {
        public int ParamId { get; set; }
        public bool KetQua { get; set; }
        public string? GhiChu { get; set; }
    }

    public class BaoVeKiemTraRequest
    {
        public string Sothe { get; set; } = string.Empty;
        public List<VesinhAnswerDto> Answers { get; set; } = new();
        public string? ChecklistData { get; set; }
    }

    [HttpPost("baove-kiemtra")]
    public async Task<IActionResult> BaoVeKiemTra([FromBody] BaoVeKiemTraRequest request)
    {
        var sothe = request.Sothe?.Trim();
        if (string.IsNullOrEmpty(sothe)) return BadRequest(new { message = "Số thẻ không hợp lệ" });

        var today = DateTime.UtcNow.AddHours(7).Date;
        var record = await _context.Danhsachxetrongkhos
            .FirstOrDefaultAsync(x => x.Sothe == sothe && x.GioDangKy.Date == today);

        if (record == null) return NotFound(new { message = "Không tìm thấy xe trong danh sách" });

        var driver = await _context.Thenhathaus.FirstOrDefaultAsync(t => t.Sothe.Trim() == sothe);
        
        // Save Pre-trip if provided
        if (!string.IsNullOrEmpty(request.ChecklistData))
        {
            var checklistRecord = new DailyPreTripChecklist
            {
                Id = Guid.NewGuid(),
                Date = DateTime.UtcNow.AddHours(7),
                MaLx = driver?.MaLx ?? "",
                TenLx = driver?.Ten ?? "",
                MaNvt = driver?.MaNvt ?? "",
                TenNvt = driver?.TenNvt ?? "",
                Sothe = sothe,
                ChecklistData = request.ChecklistData
            };
            _context.DailyPreTripChecklists.Add(checklistRecord);
            
            // Đánh dấu là đã kiểm tra PreTrip hôm nay
            record.BaoVeKiemTra_PreTrip_Time = DateTime.UtcNow.AddHours(7);
        }

        // Save Ve Sinh if provided
        if (request.Answers != null && request.Answers.Count > 0)
        {
            var activeParams = await _context.KiemtravesinhxeParams.Where(p => p.IsActive).ToListAsync();
            var details = new List<DailyKiemtravesinhxeDetail>();
            
            foreach(var param in activeParams)
            {
                var answer = request.Answers.FirstOrDefault(a => a.ParamId == param.Id);
                if (answer == null) return BadRequest(new { message = $"Thiếu câu trả lời cho mục: {param.NoiDung}" });
                if (answer.KetQua != param.DapAnDat) return BadRequest(new { message = "Không đạt tiêu chuẩn vệ sinh, vui lòng báo tài xế xử lý lại." });

                details.Add(new DailyKiemtravesinhxeDetail
                {
                    ParamId = param.Id,
                    KetQua = answer.KetQua,
                    GhiChu = answer.GhiChu
                });
            }

            var lanVaoKho = await _context.DailyKiemtravesinhxes
                .Where(d => d.Sothe == sothe && d.NgayKiemTra.Date == today).CountAsync() + 1;

            var hygieneCheck = new DailyKiemtravesinhxe
            {
                TenNhaThau = driver?.TenNvt ?? "",
                TenLaiXe = driver?.Ten ?? "",
                MaLx = driver?.MaLx ?? "",
                MaNvt = driver?.MaNvt ?? "",
                Sothe = sothe,
                NgayKiemTra = DateTime.UtcNow.AddHours(7),
                LanVaoKhoTrongNgay = lanVaoKho,
                Details = details
            };
            _context.DailyKiemtravesinhxes.Add(hygieneCheck);
        }

        record.BaoVeKiemTra_Time = DateTime.UtcNow.AddHours(7);
        _context.Entry(record).State = EntityState.Modified;

        await _context.SaveChangesAsync();
        return Ok(new { message = "Lưu kết quả kiểm tra thành công" });
    }

    [HttpPost("update-trave-details")]
    public async Task<IActionResult> UpdateTraVeDetails([FromBody] UpdateTraVeDetailsRequest req)
    {
        var chungTus = await _context.ChungTuVaoKhos
            .Where(c => c.Sothe == req.Sothe && c.SoShipment == req.SoShipment && c.LyDo == "Nhập hàng trả về")
            .ToListAsync();

        if (chungTus.Count == 0) return NotFound(new { message = "Không tìm thấy chứng từ" });

        foreach (var c in chungTus)
        {
            c.LyDoTraVe = req.LyDoTraVe;
            c.MaKhachHang = req.MaKhachHang;
        }

        await _context.SaveChangesAsync();
        return Ok(new { message = "Đã cập nhật" });
    }

    public class UpdateTraVeDetailsRequest
    {
        public string Sothe { get; set; }
        public string SoShipment { get; set; }
        public string LyDoTraVe { get; set; }
        public string MaKhachHang { get; set; }
    }
}
