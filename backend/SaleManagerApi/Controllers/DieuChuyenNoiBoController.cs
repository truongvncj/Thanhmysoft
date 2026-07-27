using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleManagerApi.Data;
using SaleManagerApi.Models;

namespace SaleManagerApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DieuChuyenNoiBoController : ControllerBase
{
    private readonly AppDbContext _context;

    public DieuChuyenNoiBoController(AppDbContext context)
    {
        _context = context;
    }

    // Lấy tồn kho theo Vị trí (dùng cho Điều chuyển)
    [HttpGet("Kho/{khohangId}/ViTri/{viTri}")]
    public async Task<ActionResult<IEnumerable<TonKhoHienTai>>> GetTonKhoByViTri(int khohangId, string viTri)
    {
        return await _context.TonKhoHienTais
            .Where(t => t.KhohangId == khohangId && t.ViTri == viTri && (t.SoLuongPalletChan > 0 || t.SoThungLe > 0))
            .OrderBy(t => t.MaHang)
            .ThenBy(t => t.TenSanPham)
            .ThenBy(t => t.HanSuDung ?? DateTime.MaxValue)
            .ThenBy(t => t.NgaySanXuat ?? DateTime.MaxValue)
            .ToListAsync();
    }

    // Tạo lệnh điều chuyển nội bộ
    [HttpPost("Create")]
    public async Task<ActionResult<LenhDieuChuyenNoiBo>> CreateLenh([FromBody] LenhDieuChuyenNoiBo lenh)
    {
        if (lenh.ChiTiets == null || lenh.ChiTiets.Count == 0)
        {
            return BadRequest("Lệnh không có chi tiết hàng hoá.");
        }

        lenh.ThoiGianTao = DateTime.UtcNow.AddHours(7);
        lenh.ThoiGianTao = DateTime.SpecifyKind(lenh.ThoiGianTao, DateTimeKind.Utc);
        lenh.TrangThai = "Pending";
        
        foreach (var ct in lenh.ChiTiets)
        {
            if (ct.NgaySanXuat.HasValue)
                ct.NgaySanXuat = DateTime.SpecifyKind(ct.NgaySanXuat.Value, DateTimeKind.Utc);
            if (ct.HanSuDung.HasValue)
                ct.HanSuDung = DateTime.SpecifyKind(ct.HanSuDung.Value, DateTimeKind.Utc);
        }
        
        // Sinh mã lệnh: DCNB-yyMMdd-xxx
        var today = DateTime.UtcNow.AddHours(7).ToString("yyMMdd");
        var latestLenh = await _context.LenhDieuChuyenNoiBos
            .Where(l => l.MaLenh.StartsWith($"DCNB-{today}"))
            .OrderByDescending(l => l.Id)
            .FirstOrDefaultAsync();

        int nextNumber = 1;
        if (latestLenh != null)
        {
            var parts = latestLenh.MaLenh.Split('-');
            if (parts.Length == 3 && int.TryParse(parts[2], out int lastNum))
            {
                nextNumber = lastNum + 1;
            }
        }
        lenh.MaLenh = $"DCNB-{today}-{nextNumber:D3}";

        _context.LenhDieuChuyenNoiBos.Add(lenh);
        await _context.SaveChangesAsync();

        return Ok(lenh);
    }

    // Lấy danh sách lệnh Pending
    [HttpGet("Pending/{khohangId}")]
    public async Task<ActionResult<IEnumerable<LenhDieuChuyenNoiBo>>> GetPendingLenh(int khohangId)
    {
        return await _context.LenhDieuChuyenNoiBos
            .Include(l => l.ChiTiets)
            .Where(l => l.KhohangId == khohangId && l.TrangThai == "Pending")
            .OrderByDescending(l => l.ThoiGianTao)
            .ToListAsync();
    }

    // Hoàn thành lệnh điều chuyển
    [HttpPost("Complete/{id}")]
    public async Task<IActionResult> CompleteLenh(int id, [FromQuery] string nguoiHoanThanh)
    {
        var lenh = await _context.LenhDieuChuyenNoiBos
            .Include(l => l.ChiTiets)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (lenh == null) return NotFound("Không tìm thấy lệnh.");
        if (lenh.TrangThai != "Pending") return BadRequest("Lệnh đã hoàn thành hoặc huỷ.");

        lenh.TrangThai = "Completed";
        lenh.ThoiGianHoanThanh = DateTime.UtcNow.AddHours(7);
        lenh.NguoiHoanThanh = nguoiHoanThanh;

        // Tạo PhatSinhSanPham cho Xuất và Nhập
        foreach (var chiTiet in lenh.ChiTiets)
        {
            // 1. Dòng Xuất (ở vị trí đi)
            var phatSinhXuat = new PhatSinhSanPham
            {
                MaSanPham = chiTiet.MaSanPham,
                TenSanPham = chiTiet.TenSanPham,
                LoaiPhatSinh = "Xuất",
                SoLuongChan = chiTiet.SoLuongChan,
                SoLuongLe = chiTiet.SoLuongLe,
                ViTri = chiTiet.ViTriDi,
                NgaySanXuat = chiTiet.NgaySanXuat,
                HanSuDung = chiTiet.HanSuDung,
                User = nguoiHoanThanh, // Hoặc NguoiTao tuỳ chọn
                TenNhanVien = nguoiHoanThanh,
                ThoiGian = lenh.ThoiGianHoanThanh.Value,
                SoSTO = $"DCNB_{lenh.MaLenh}",
                Status = "ok"
            };
            
            // 2. Dòng Nhập (ở vị trí đến)
            var phatSinhNhap = new PhatSinhSanPham
            {
                MaSanPham = chiTiet.MaSanPham,
                TenSanPham = chiTiet.TenSanPham,
                LoaiPhatSinh = "Nhập",
                SoLuongChan = chiTiet.SoLuongChan,
                SoLuongLe = chiTiet.SoLuongLe,
                ViTri = chiTiet.ViTriDen,
                NgaySanXuat = chiTiet.NgaySanXuat,
                HanSuDung = chiTiet.HanSuDung,
                User = nguoiHoanThanh,
                TenNhanVien = nguoiHoanThanh,
                ThoiGian = lenh.ThoiGianHoanThanh.Value,
                SoSTO = $"DCNB_{lenh.MaLenh}",
                Status = "ok"
            };

            _context.PhatSinhSanPhams.Add(phatSinhXuat);
            _context.PhatSinhSanPhams.Add(phatSinhNhap);
        }

        await _context.SaveChangesAsync();

        return Ok(lenh);
    }
}
