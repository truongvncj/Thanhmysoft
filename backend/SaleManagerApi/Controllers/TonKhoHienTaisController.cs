using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleManagerApi.Data;
using SaleManagerApi.Models;

namespace SaleManagerApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TonKhoHienTaisController : ControllerBase
{
    private readonly AppDbContext _context;

    public TonKhoHienTaisController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("BySanPham/{maSanPham}")]
    public async Task<ActionResult<IEnumerable<TonKhoHienTai>>> GetTonKhoBySanPham(string maSanPham)
    {
        // Get all locations where this product has stock
        return await _context.TonKhoHienTais
            .Where(t => t.MaHang == maSanPham && (t.SoLuongPalletChan > 0 || t.SoThungLe > 0))
            .OrderBy(t => t.KhohangId)
            .ThenBy(t => t.ViTri)
            .ToListAsync();
    }

    [HttpGet("ByKhohang/{khohangId}")]
    public async Task<ActionResult<IEnumerable<TonKhoHienTai>>> GetTonKhoByKhohang(int khohangId)
    {
        // Get all products with stock in a specific warehouse
        return await _context.TonKhoHienTais
            .Where(t => t.KhohangId == khohangId && (t.SoLuongPalletChan > 0 || t.SoThungLe > 0))
            .OrderBy(t => t.ViTri)
            .ThenBy(t => t.MaHang)
            .ToListAsync();
    }
}
