using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleManagerApi.Data;
using SaleManagerApi.Models;

namespace SaleManagerApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PhatSinhSanPhamsController : ControllerBase
{
    private readonly AppDbContext _context;


    public PhatSinhSanPhamsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("ByDateAndKhohang")]
    public async Task<IActionResult> GetByDateAndKhohang([FromQuery] int khohangId, [FromQuery] string date)
    {
        if (!DateTime.TryParse(date, out var parsedDate))
        {
            return BadRequest(new { message = "Ngày không hợp lệ" });
        }
        
        var targetDate = DateTime.SpecifyKind(parsedDate.Date, DateTimeKind.Utc);
        var nextDate = targetDate.AddDays(1);

        var data = await _context.PhatSinhSanPhams
            .Where(p => p.KhohangId == khohangId && p.ThoiGian >= targetDate && p.ThoiGian < nextDate)
            .OrderByDescending(p => p.ThoiGian)
            .ToListAsync();

        return Ok(data);
    }

    [HttpGet("BySothe")]
    public async Task<IActionResult> GetBySothe([FromQuery] string sothe, [FromQuery] string? loai)
    {
        var today = DateTime.UtcNow.AddHours(7).Date;
        var tomorrow = today.AddDays(1);
        
        var query = _context.PhatSinhSanPhams
            .Where(p => p.Sothe == sothe && p.ThoiGian >= today && p.ThoiGian < tomorrow);
            
        if (!string.IsNullOrEmpty(loai))
        {
            query = query.Where(p => p.LoaiPhatSinh == loai);
        }
        
        var data = await query
            .OrderBy(p => p.MaSanPham).ThenBy(p => p.ViTri)
            .ToListAsync();
            
        return Ok(data);
    }

    [HttpPost("XuatKho")]
    public async Task<IActionResult> XuatKho([FromBody] PhatSinhSanPham request)
    {
        request.LoaiPhatSinh = "Xuất";
        request.Status = "pending";
        request.ThoiGian = DateTime.UtcNow.AddHours(7);
        
        // Find existing inventory in TonKhoHienTai
        var tonKho = await _context.TonKhoHienTais.FirstOrDefaultAsync(t => 
            t.MaHang == request.MaSanPham && 
            t.ViTri == request.ViTri);

        if (tonKho == null)
        {
            return BadRequest(new { message = "Không tìm thấy sản phẩm tại vị trí này trong tồn kho hiện tại." });
        }

        // Deduct quantities
        int xuatChan = request.SoLuongChan ?? 0;
        int xuatLe = request.SoLuongLe ?? 0;

        if (tonKho.SoLuongPalletChan < xuatChan || tonKho.SoThungLe < xuatLe)
        {
            return BadRequest(new { message = "Số lượng xuất vượt quá tồn kho hiện tại." });
        }

        tonKho.SoLuongPalletChan -= xuatChan;
        tonKho.SoThungLe -= xuatLe;
        
        // Ensure Tong is also updated if needed (Assuming DinhLuong is static or known)
        if (tonKho.DinhLuong.HasValue)
        {
            tonKho.Tong = (tonKho.SoLuongPalletChan * tonKho.DinhLuong.Value) + tonKho.SoThungLe;
        }

        _context.Entry(tonKho).State = EntityState.Modified;

        request.Status = "ok";
        _context.PhatSinhSanPhams.Add(request);

        await _context.SaveChangesAsync();

        return Ok(new { message = "Xuất kho thành công.", data = request });
    }
    [HttpPost("NhapKho")]
    public async Task<IActionResult> NhapKho([FromBody] PhatSinhSanPham request)
    {
        request.LoaiPhatSinh = "Nhập";
        request.Status = "ok";
        request.ThoiGian = DateTime.UtcNow.AddHours(7);
        
        if (request.NgaySanXuat.HasValue)
        {
            request.NgaySanXuat = DateTime.SpecifyKind(request.NgaySanXuat.Value, DateTimeKind.Utc);
        }
        if (request.HanSuDung.HasValue)
        {
            request.HanSuDung = DateTime.SpecifyKind(request.HanSuDung.Value, DateTimeKind.Utc);
        }

        // Find existing inventory in TonKhoHienTai
        // Group by exact product, location, manufacturing date, and expiration date
        var tonKho = await _context.TonKhoHienTais.FirstOrDefaultAsync(t => 
            t.MaHang == request.MaSanPham && 
            t.ViTri == request.ViTri &&
            t.HanSuDung == request.HanSuDung &&
            t.NgaySanXuat == request.NgaySanXuat);

        int nhapChan = request.SoLuongChan ?? 0;
        int nhapLe = request.SoLuongLe ?? 0;

        if (tonKho != null)
        {
            tonKho.SoLuongPalletChan += nhapChan;
            tonKho.SoThungLe += nhapLe;
            
            if (tonKho.DinhLuong.HasValue)
            {
                tonKho.Tong = (tonKho.SoLuongPalletChan * tonKho.DinhLuong.Value) + tonKho.SoThungLe;
            }
            _context.Entry(tonKho).State = EntityState.Modified;
        }
        else
        {
            // Lookup DinhLuong from Sanpham
            var productInfo = await _context.Sanphams
                .FirstOrDefaultAsync(p => p.MaSanPham == request.MaSanPham);
            
            int dinhLuong = productInfo?.DinhLuong ?? 0;
            
            tonKho = new TonKhoHienTai
            {
                ViTri = request.ViTri,
                MaHang = request.MaSanPham,
                TenSanPham = request.TenSanPham,
                SoLuongPalletChan = nhapChan,
                SoThungLe = nhapLe,
                DinhLuong = dinhLuong,
                Tong = (nhapChan * dinhLuong) + nhapLe,
                KhohangId = request.KhohangId ?? 1,
                HanSuDung = request.HanSuDung,
                NgaySanXuat = request.NgaySanXuat
            };
            _context.TonKhoHienTais.Add(tonKho);
        }

        _context.PhatSinhSanPhams.Add(request);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Nhập kho thành công.", data = request });
    }
}
