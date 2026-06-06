using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaleManagerApi.Models;

[Table("nhanvien")]
public class Nhanvien
{
    [Key]
    [Column("mnv")]
    public string Mnv { get; set; } = string.Empty;

    [Column("tnv")]
    public string Tnv { get; set; } = string.Empty;

    [Column("chucdanh")]
    public string Chucdanh { get; set; } = string.Empty;

    [Column("vitri")]
    public string Vitri { get; set; } = string.Empty; // "Nhân viên" hoặc "Bảo vệ"

    [Column("password")]
    public string? Password { get; set; } = string.Empty;

    [Column("permissions")]
    public string? Permissions { get; set; } = string.Empty; // Danh sách mã quyền cách nhau bởi dấu phẩy
}
