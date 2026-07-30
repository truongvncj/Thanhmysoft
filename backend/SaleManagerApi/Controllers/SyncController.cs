using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleManagerApi.Data;
using SaleManagerApi.Models;

namespace SaleManagerApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SyncController : ControllerBase
{
    private readonly AppDbContext _context;

    public SyncController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("TonKho/{khohangId}")]
    public async Task<IActionResult> SyncTonKho(int khohangId)
    {
        // 1. Delete current TonKhoHienTai for this khohang
        var current = await _context.TonKhoHienTais.Where(t => t.KhohangId == khohangId).ToListAsync();
        _context.TonKhoHienTais.RemoveRange(current);
        
        // 2. Get all TonKhoDauKy for this khohang
        var dauKy = await _context.TonKhoDauKies.Where(t => t.KhohangId == khohangId).ToListAsync();
        
        // 3. Map to TonKhoHienTai
        var hienTaiDict = new Dictionary<string, TonKhoHienTai>(); // key: ViTri_MaHang
        
        foreach (var dk in dauKy)
        {
            string nsx = dk.NgaySanXuat?.ToString("yyyyMMdd") ?? "";
            string hsd = dk.HanSuDung?.ToString("yyyyMMdd") ?? "";
            var key = $"{dk.ViTri}_{dk.MaHang}_{nsx}_{hsd}";
            if (!hienTaiDict.ContainsKey(key))
            {
                hienTaiDict[key] = new TonKhoHienTai
                {
                    KhohangId = dk.KhohangId,
                    ViTri = dk.ViTri,
                    MaHang = dk.MaHang,
                    TenSanPham = dk.TenSanPham,
                    HanSuDung = dk.HanSuDung,
                    PhanTramHanSuDung = dk.PhanTramHanSuDung,
                    SoLuongPalletChan = dk.SoLuongPalletChan ?? 0,
                    SoThungLe = dk.SoThungLe ?? 0,
                    DinhLuong = dk.DinhLuong,
                    Tong = dk.Tong ?? 0,
                    GhiChu = dk.GhiChu,
                    TrangThaiDOD = dk.TrangThaiDOD,
                    NgaySanXuat = dk.NgaySanXuat,
                    DayHang = dk.DayHang,
                    SttDayHang = dk.SttDayHang,
                    ThoiDiemGhiNhan = dk.ThoiDiemGhiNhan
                };
            }
            else
            {
                hienTaiDict[key].SoLuongPalletChan += (dk.SoLuongPalletChan ?? 0);
                hienTaiDict[key].SoThungLe += (dk.SoThungLe ?? 0);
                hienTaiDict[key].Tong += (dk.Tong ?? 0);
            }
        }
        
        // 4. Get all PhatSinhSanPham for this khohang and apply them
        var phatSinh = await _context.PhatSinhSanPhams
            .Where(p => p.Status == "ok" || p.Status == "pending") // Adjust status if needed
            .ToListAsync();
            
        // Filter by khohang if PhatSinhSanPham has KhohangId, but it doesn't currently! 
        // We will just match by ViTri and MaHang for the ones in our dictionary
        
        foreach(var ps in phatSinh)
        {
            string nsx = ps.NgaySanXuat?.ToString("yyyyMMdd") ?? "";
            string hsd = ps.HanSuDung?.ToString("yyyyMMdd") ?? "";
            var key = $"{ps.ViTri}_{ps.MaSanPham}_{nsx}_{hsd}";
            
            if (hienTaiDict.ContainsKey(key))
            {
                var hienTai = hienTaiDict[key];
                int chan = ps.SoLuongChan ?? 0;
                int le = ps.SoLuongLe ?? 0;
                
                if (ps.LoaiPhatSinh == "Nhập")
                {
                    hienTai.SoLuongPalletChan += chan;
                    hienTai.SoThungLe += le;
                }
                else if (ps.LoaiPhatSinh == "Xuất")
                {
                    hienTai.SoLuongPalletChan -= chan;
                    hienTai.SoThungLe -= le;
                }
                
                if (hienTai.DinhLuong.HasValue)
                {
                    hienTai.Tong = (hienTai.SoLuongPalletChan * hienTai.DinhLuong.Value) + hienTai.SoThungLe;
                }
            }
            else if (ps.LoaiPhatSinh == "Nhập")
            {
                // If importing a new batch that didn't exist in DauKy
                hienTaiDict[key] = new TonKhoHienTai
                {
                    KhohangId = khohangId, // Warning: assumes PhatSinh is for this khohang.
                    ViTri = ps.ViTri,
                    MaHang = ps.MaSanPham,
                    TenSanPham = ps.TenSanPham,
                    NgaySanXuat = ps.NgaySanXuat,
                    HanSuDung = ps.HanSuDung,
                    SoLuongPalletChan = ps.SoLuongChan ?? 0,
                    SoThungLe = ps.SoLuongLe ?? 0,
                    Tong = 0
                };
            }
        }
        
        // Fill missing product names from Sanphams
        var missingNames = hienTaiDict.Values.Where(t => string.IsNullOrWhiteSpace(t.TenSanPham)).ToList();
        if (missingNames.Any())
        {
            var productCodes = missingNames.Select(t => t.MaHang).Distinct().ToList();
            var products = await _context.Sanphams
                .Where(p => productCodes.Contains(p.MaSanPham))
                .GroupBy(p => p.MaSanPham)
                .Select(g => new { MaSanPham = g.Key, TenSanPham = g.First().TenSanPham })
                .ToDictionaryAsync(p => p.MaSanPham, p => p.TenSanPham);

            foreach (var tk in missingNames)
            {
                if (products.TryGetValue(tk.MaHang, out var prodName))
                {
                    tk.TenSanPham = prodName;
                }
            }
        }

        // 5. Save
        _context.TonKhoHienTais.AddRange(hienTaiDict.Values);
        await _context.SaveChangesAsync();
        
        return Ok(new { message = "Đồng bộ tồn kho thành công." });
    }
}
