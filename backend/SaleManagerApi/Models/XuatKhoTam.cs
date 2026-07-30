using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaleManagerApi.Models;

[Table("xuatkhotam")]
public class XuatKhoTam
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string MaNhanVien { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string SoChungTu { get; set; } = string.Empty; // STO or Shipment

    [Required]
    [MaxLength(255)]
    public string MaSanPham { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? TenSanPham { get; set; }

    [MaxLength(255)]
    public string? ViTri { get; set; }

    public int SoLuongChan { get; set; }

    public int SoLuongLe { get; set; }

    public DateTime? NgaySanXuat { get; set; }

    public DateTime? HanSuDung { get; set; }

    public int? KhohangId { get; set; }
}
