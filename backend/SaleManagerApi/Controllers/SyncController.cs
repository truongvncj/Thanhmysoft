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
            var key = $"{dk.ViTri}_{dk.MaHang}";
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
            var key = $"{ps.ViTri}_{ps.MaSanPham}";
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
        }
        
        // 5. Save
        _context.TonKhoHienTais.AddRange(hienTaiDict.Values);
        await _context.SaveChangesAsync();
        
        return Ok(new { message = "Đồng bộ tồn kho thành công." });
    }
}
