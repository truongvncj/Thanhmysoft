using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaleManagerApi.Models;

[Table("admin")]
public class Admin
{
    [Key]
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("password")]
    public string Password { get; set; } = string.Empty;

    [Column("right")]
    public int Right { get; set; }
}
