using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SaleManagerApi.Models;

public class ChiTietDieuChuyenNoiBo
{
    [Key]
    public int Id { get; set; }

    public int LenhDieuChuyenNoiBoId { get; set; }
    
    [JsonIgnore]
    [ForeignKey("LenhDieuChuyenNoiBoId")]
    public LenhDieuChuyenNoiBo? LenhDieuChuyen { get; set; }

    [Required]
    [StringLength(50)]
    public string MaSanPham { get; set; } = string.Empty;

    [StringLength(255)]
    public string? TenSanPham { get; set; }

    public DateTime? NgaySanXuat { get; set; }
    public DateTime? HanSuDung { get; set; }

    [Required]
    [StringLength(50)]
    public string ViTriDi { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string ViTriDen { get; set; } = string.Empty;

    public int SoLuongChan { get; set; } = 0;
    public int SoLuongLe { get; set; } = 0;
}
