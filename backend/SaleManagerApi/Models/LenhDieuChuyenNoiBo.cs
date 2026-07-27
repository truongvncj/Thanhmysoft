using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaleManagerApi.Models;

public class LenhDieuChuyenNoiBo
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string MaLenh { get; set; } = string.Empty;

    public int KhohangId { get; set; }
    
    [ForeignKey("KhohangId")]
    public Khohang? Khohang { get; set; }

    public DateTime ThoiGianTao { get; set; } = DateTime.UtcNow.AddHours(7);

    [StringLength(50)]
    public string NguoiTao { get; set; } = string.Empty; // Tên nhân viên tạo lệnh

    [StringLength(20)]
    public string TrangThai { get; set; } = "Pending"; // Pending, Completed, Cancelled

    public DateTime? ThoiGianHoanThanh { get; set; }
    
    [StringLength(50)]
    public string? NguoiHoanThanh { get; set; }

    public List<ChiTietDieuChuyenNoiBo> ChiTiets { get; set; } = new List<ChiTietDieuChuyenNoiBo>();
}
