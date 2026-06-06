using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaleManagerApi.Models;

[Table("thenhathau")]
public class Thenhathau
{
    [Key]
    [Column("malx")]
    public string MaLx { get; set; } = string.Empty;

    [Column("manvt")]
    public string MaNvt { get; set; } = string.Empty;

    [Column("tennvt")]
    public string TenNvt { get; set; } = string.Empty;

    [Column("ten")]
    public string Ten { get; set; } = string.Empty;

    [Column("cccd")]
    public string Cccd { get; set; } = string.Empty;

    [Column("sodt")]
    public string Sodt { get; set; } = string.Empty;

    [Column("sothe")]
    public string Sothe { get; set; } = string.Empty;

    [Column("bienso")]
    public string BienSo { get; set; } = string.Empty;

    [Column("thoihanthe")]
    public DateTime Thoihanthe { get; set; }

    [Column("trangthai")]
    public int TrangThai { get; set; } = 1; // 0: Chờ duyệt, 1: Đã duyệt/Hoạt động, 2: Bị khoá
}
