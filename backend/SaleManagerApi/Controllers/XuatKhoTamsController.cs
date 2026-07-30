using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleManagerApi.Data;
using SaleManagerApi.Models;

namespace SaleManagerApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class XuatKhoTamsController : ControllerBase
{
    private readonly AppDbContext _context;

    public XuatKhoTamsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<XuatKhoTam>>> GetXuatKhoTams(
        [FromQuery] string soChungTu, 
        [FromQuery] string maNhanVien)
    {
        return await _context.XuatKhoTams
            .Where(x => x.SoChungTu == soChungTu && x.MaNhanVien == maNhanVien)
            .OrderBy(x => x.MaSanPham)
            .ThenBy(x => x.ViTri)
            .ToListAsync();
    }

    [HttpPost]
    public async Task<IActionResult> AddOrUpdateXuatKhoTam([FromBody] XuatKhoTam request)
    {
        if (request.NgaySanXuat.HasValue)
        {
            request.NgaySanXuat = DateTime.SpecifyKind(request.NgaySanXuat.Value, DateTimeKind.Utc);
        }
        if (request.HanSuDung.HasValue)
        {
            request.HanSuDung = DateTime.SpecifyKind(request.HanSuDung.Value, DateTimeKind.Utc);
        }

        // Find existing inventory in TonKhoHienTai
        var tonKho = await _context.TonKhoHienTais.FirstOrDefaultAsync(t =>
            t.MaHang == request.MaSanPham &&
            t.ViTri == request.ViTri &&
            t.NgaySanXuat == request.NgaySanXuat &&
            t.HanSuDung == request.HanSuDung &&
            t.KhohangId == request.KhohangId);

        if (tonKho == null)
        {
            return BadRequest(new { message = "Không tìm thấy sản phẩm này trong tồn kho." });
        }

        // Calculate other users' / other documents' reservations
        var reservedChan = await _context.XuatKhoTams
            .Where(x => x.MaSanPham == request.MaSanPham &&
                        x.ViTri == request.ViTri &&
                        x.NgaySanXuat == request.NgaySanXuat &&
                        x.HanSuDung == request.HanSuDung &&
                        x.KhohangId == request.KhohangId &&
                        !(x.MaNhanVien == request.MaNhanVien && x.SoChungTu == request.SoChungTu))
            .SumAsync(x => x.SoLuongChan);

        var reservedLe = await _context.XuatKhoTams
            .Where(x => x.MaSanPham == request.MaSanPham &&
                        x.ViTri == request.ViTri &&
                        x.NgaySanXuat == request.NgaySanXuat &&
                        x.HanSuDung == request.HanSuDung &&
                        x.KhohangId == request.KhohangId &&
                        !(x.MaNhanVien == request.MaNhanVien && x.SoChungTu == request.SoChungTu))
            .SumAsync(x => x.SoLuongLe);

        int availableChan = (tonKho.SoLuongPalletChan ?? 0) - reservedChan;
        int availableLe = (tonKho.SoThungLe ?? 0) - reservedLe;

        // Find existing entry for this user and document
        var existing = await _context.XuatKhoTams.FirstOrDefaultAsync(x =>
            x.MaNhanVien == request.MaNhanVien &&
            x.SoChungTu == request.SoChungTu &&
            x.MaSanPham == request.MaSanPham &&
            x.ViTri == request.ViTri &&
            x.NgaySanXuat == request.NgaySanXuat &&
            x.HanSuDung == request.HanSuDung &&
            x.KhohangId == request.KhohangId);

        int newChan = (existing?.SoLuongChan ?? 0) + request.SoLuongChan;
        int newLe = (existing?.SoLuongLe ?? 0) + request.SoLuongLe;

        if (newChan > availableChan || newLe > availableLe)
        {
            return BadRequest(new { message = $"Không đủ tồn kho khả dụng tại vị trí {request.ViTri}. Tồn khả dụng: {availableChan} chẵn, {availableLe} lẻ. Số lượng yêu cầu: {newChan} chẵn, {newLe} lẻ." });
        }

        if (existing != null)
        {
            existing.SoLuongChan = newChan;
            existing.SoLuongLe = newLe;
            _context.Entry(existing).State = EntityState.Modified;
        }
        else
        {
            _context.XuatKhoTams.Add(request);
        }

        await _context.SaveChangesAsync();
        return Ok(new { message = "Thêm vào phiếu Load thành công.", data = request });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteXuatKhoTam(int id)
    {
        var item = await _context.XuatKhoTams.FindAsync(id);
        if (item == null)
        {
            return NotFound();
        }

        _context.XuatKhoTams.Remove(item);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Xóa mặt hàng tạm tính thành công." });
    }

    [HttpPost("Clear")]
    public async Task<IActionResult> ClearXuatKhoTams(
        [FromQuery] string soChungTu, 
        [FromQuery] string maNhanVien)
    {
        var items = await _context.XuatKhoTams
            .Where(x => x.SoChungTu == soChungTu && x.MaNhanVien == maNhanVien)
            .ToListAsync();

        if (items.Any())
        {
            _context.XuatKhoTams.RemoveRange(items);
            await _context.SaveChangesAsync();
        }

        return Ok(new { message = "Đã dọn dẹp các mặt hàng tạm tính." });
    }
}
