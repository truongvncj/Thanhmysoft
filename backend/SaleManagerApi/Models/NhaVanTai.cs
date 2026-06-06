using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaleManagerApi.Models;

[Table("nhavantai")]
public class NhaVanTai
{
    [Key]
    [Column("manvt")]
    public string MaNvt { get; set; } = string.Empty;

    [Column("tennvt")]
    public string TenNvt { get; set; } = string.Empty;

    [Column("password")]
    public string Password { get; set; } = string.Empty;
}
