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
        var list = await _context.TonKhoHienTais
            .Where(t => t.MaHang == maSanPham && (t.SoLuongPalletChan > 0 || t.SoThungLe > 0))
            .OrderBy(t => t.HanSuDung ?? DateTime.MaxValue) // Null HSD goes last
            .ThenBy(t => t.KhohangId)
            .ThenBy(t => t.ViTri)
            .ToListAsync();

        var tempPicks = await _context.XuatKhoTams
            .Where(x => x.MaSanPham == maSanPham)
            .ToListAsync();

        var result = new List<TonKhoHienTai>();

        foreach (var tk in list)
        {
            var matchedPicks = tempPicks.Where(x =>
                x.ViTri == tk.ViTri &&
                x.NgaySanXuat == tk.NgaySanXuat &&
                x.HanSuDung == tk.HanSuDung &&
                x.KhohangId == tk.KhohangId);

            int reservedChan = matchedPicks.Sum(x => x.SoLuongChan);
            int reservedLe = matchedPicks.Sum(x => x.SoLuongLe);

            tk.SoLuongPalletChan = (tk.SoLuongPalletChan ?? 0) - reservedChan;
            tk.SoThungLe = (tk.SoThungLe ?? 0) - reservedLe;

            // Only return locations that still have positive available stock
            if (tk.SoLuongPalletChan > 0 || tk.SoThungLe > 0)
            {
                result.Add(tk);
            }
        }

        return result;
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
