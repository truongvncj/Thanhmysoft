using System.ComponentModel.DataAnnotations;

namespace SaleManagerApi.Models;

public class DangTai
{
    [Key]
    public Guid Id { get; set; }
    
    public string MaLx { get; set; } = string.Empty;
    public string Sothe { get; set; } = string.Empty;
    public DateTime NgayDangTai { get; set; }
    public bool DaDangTai { get; set; }
    public bool DaVeSinh { get; set; }
    public string TenNvt { get; set; } = string.Empty;
    
    public bool DaDangKyChungTu { get; set; }
    
    // TrangThai: 0: Đã đăng ký, 1: Trong sân, 2: Chuẩn bị ra sân, 3: Đã hoàn thành
    public int TrangThai { get; set; } = 0; 
    
    public int? KhohangId { get; set; }
    
    // Lý do: Nhập hàng / Xuất hàng
    public string LyDo { get; set; } = string.Empty;

    public string BienSo { get; set; } = string.Empty;
    
    public string? LyDoHuy { get; set; }
}
