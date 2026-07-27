using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaleManagerApi.Models;

[Table("sanphamnuoc")]
public class Sanphamnuoc
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int KhohangId { get; set; }

    [ForeignKey("KhohangId")]
    public Khohang? Khohang { get; set; }

    [Required]
    [MaxLength(255)]
    public string MaSanPham { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string TenSanPham { get; set; } = string.Empty;

    public int? DinhLuong { get; set; }
}
