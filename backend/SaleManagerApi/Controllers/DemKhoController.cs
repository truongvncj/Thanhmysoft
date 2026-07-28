using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleManagerApi.Data;
using SaleManagerApi.Models;

namespace SaleManagerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DemKhoController : ControllerBase
{
    private readonly AppDbContext _context;

    public DemKhoController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("Kho/{khohangId}")]
    public async Task<ActionResult<IEnumerable<PhieuDemKho>>> GetPhieuDemKhos(int khohangId)
    {
        return await _context.PhieuDemKhos
            .Include(p => p.ChiTiets)
            .Where(p => p.KhohangId == khohangId)
            .OrderByDescending(p => p.ThoiGianTao)
            .ToListAsync();
    }

    [HttpPost("Create")]
    public async Task<ActionResult<PhieuDemKho>> CreatePhieuDemKho([FromBody] PhieuDemKho phieu)
    {
        if (phieu == null || phieu.ChiTiets == null || !phieu.ChiTiets.Any())
        {
            return BadRequest("Dữ liệu phiếu đếm không hợp lệ.");
        }

        // Tính toán lại chênh lệch cho chắc chắn (Số đếm - Số hiện tại)
        foreach (var ct in phieu.ChiTiets)
        {
            ct.ChenhLechChan = ct.SoDemChan - ct.TonChanHienTai;
            ct.ChenhLechLe = ct.SoDemLe - ct.TonLeHienTai;
            
            // Xóa timestamp offset (chuyển sang UTC format nếu cần)
            if (ct.NgaySanXuat.HasValue)
            {
                ct.NgaySanXuat = DateTime.SpecifyKind(ct.NgaySanXuat.Value, DateTimeKind.Utc);
            }
            if (ct.HanSuDung.HasValue)
            {
                ct.HanSuDung = DateTime.SpecifyKind(ct.HanSuDung.Value, DateTimeKind.Utc);
            }
        }
        
        // Setup timezone
        phieu.ThoiGianTao = DateTime.UtcNow.AddHours(7);

        _context.PhieuDemKhos.Add(phieu);
        await _context.SaveChangesAsync();

        return Ok(phieu);
    }

    [HttpPost("Duyet/{id}")]
    public async Task<IActionResult> DuyetPhieu(int id, [FromQuery] string nguoiDuyet)
    {
        var phieu = await _context.PhieuDemKhos
            .Include(p => p.ChiTiets)
            .FirstOrDefaultAsync(p => p.Id == id);
            
        if (phieu == null) return NotFound("Không tìm thấy phiếu đếm kho.");
        if (phieu.TrangThai != "Chờ duyệt" && !string.IsNullOrEmpty(phieu.TrangThai)) 
            return BadRequest("Phiếu đếm kho đã được duyệt hoặc bị hủy.");

        phieu.TrangThai = "Duyệt";
        phieu.NguoiDuyet = nguoiDuyet ?? "Hệ thống";
        phieu.ThoiGianDuyet = DateTime.UtcNow.AddHours(7);

        foreach (var ct in phieu.ChiTiets)
        {
            if (ct.ChenhLechChan == 0 && ct.ChenhLechLe == 0) continue;

            // 1. Ghi nhận PhatSinhSanPham
            var psp = new PhatSinhSanPham
            {
                KhohangId = phieu.KhohangId,
                MaSanPham = ct.MaSanPham,
                TenSanPham = ct.TenSanPham,
                NgaySanXuat = ct.NgaySanXuat,
                HanSuDung = ct.HanSuDung,
                ViTri = phieu.ViTri,
                SoLuongChan = ct.ChenhLechChan,
                SoLuongLe = ct.ChenhLechLe,
                LoaiPhatSinh = "Đếm kho điều chỉnh",
                Status = "ok",
                ThoiGian = phieu.ThoiGianDuyet.Value
            };
            _context.PhatSinhSanPhams.Add(psp);

            // 2. Cập nhật TonKhoHienTai
            var tonKho = await _context.TonKhoHienTais.FirstOrDefaultAsync(t =>
                t.KhohangId == phieu.KhohangId &&
                t.MaHang == ct.MaSanPham &&
                t.ViTri == phieu.ViTri &&
                t.NgaySanXuat == ct.NgaySanXuat &&
                t.HanSuDung == ct.HanSuDung);

            if (tonKho != null)
            {
                tonKho.SoLuongPalletChan += ct.ChenhLechChan;
                tonKho.SoThungLe += ct.ChenhLechLe;
                
                // Tránh trường hợp âm tồn kho (tuỳ logic hệ thống, nhưng để nguyên vì chênh lệch thực tế có thể âm)
                if (tonKho.SoLuongPalletChan < 0) tonKho.SoLuongPalletChan = 0;
                if (tonKho.SoThungLe < 0) tonKho.SoThungLe = 0;

                _context.Entry(tonKho).State = EntityState.Modified;
            }
            else
            {
                // Nếu chênh lệch > 0 mà chưa có tồn kho thì tạo mới
                if (ct.ChenhLechChan > 0 || ct.ChenhLechLe > 0)
                {
                    tonKho = new TonKhoHienTai
                    {
                        KhohangId = phieu.KhohangId,
                        MaHang = ct.MaSanPham,
                        TenSanPham = ct.TenSanPham,
                        ViTri = phieu.ViTri,
                        NgaySanXuat = ct.NgaySanXuat,
                        HanSuDung = ct.HanSuDung,
                        SoLuongPalletChan = ct.ChenhLechChan > 0 ? ct.ChenhLechChan : 0,
                        SoThungLe = ct.ChenhLechLe > 0 ? ct.ChenhLechLe : 0,
                        ThoiDiemGhiNhan = phieu.ThoiGianDuyet.Value
                    };
                    _context.TonKhoHienTais.Add(tonKho);
                }
            }
        }

        await _context.SaveChangesAsync();
        return Ok(new { message = "Duyệt thành công" });
    }

    [HttpPost("KhongDuyet/{id}")]
    public async Task<IActionResult> KhongDuyetPhieu(int id, [FromQuery] string nguoiDuyet)
    {
        var phieu = await _context.PhieuDemKhos.FirstOrDefaultAsync(p => p.Id == id);
        if (phieu == null) return NotFound("Không tìm thấy phiếu đếm kho.");
        if (phieu.TrangThai != "Chờ duyệt" && !string.IsNullOrEmpty(phieu.TrangThai)) 
            return BadRequest("Phiếu đếm kho đã được duyệt hoặc bị hủy.");

        phieu.TrangThai = "Hủy";
        phieu.NguoiDuyet = nguoiDuyet ?? "Hệ thống";
        phieu.ThoiGianDuyet = DateTime.UtcNow.AddHours(7);

        await _context.SaveChangesAsync();
        return Ok(new { message = "Đã hủy phiếu đếm kho" });
    }
}
