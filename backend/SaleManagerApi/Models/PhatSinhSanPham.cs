using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaleManagerApi.Models;

[Table("phatsinhsanpham")]
public class PhatSinhSanPham
{
    [Key]
    public int Id { get; set; }

    [MaxLength(255)]
    public string? MaSanPham { get; set; }

    [MaxLength(500)]
    public string? TenSanPham { get; set; }

    public DateTime? HanSuDung { get; set; }

    public DateTime? NgaySanXuat { get; set; }

    public int? SoLuongChan { get; set; }

    public int? SoLuongLe { get; set; }

    [MaxLength(255)]
    public string? ViTri { get; set; }

    [MaxLength(50)]
    public string? LoaiPhatSinh { get; set; } // Nhập / Xuất

    [MaxLength(50)]
    public string? Status { get; set; } // pending / ok

    [MaxLength(50)]
    public string? LoaiNhapXuat { get; set; } // Thành phẩm / Vỏ / Nhập hàng trả về

    [MaxLength(255)]
    public string? User { get; set; }

    [MaxLength(255)]
    public string? TenNhanVien { get; set; }

    public DateTime? ThoiGian { get; set; }

    [MaxLength(255)]
    public string? Sothe { get; set; }

    [MaxLength(255)]
    public string? SoSTO { get; set; }
    
    [MaxLength(255)]
    public string? SoShipment { get; set; }

    public int? KhohangId { get; set; }

    [NotMapped]
    public string? LyDoTraVe { get; set; }
}
