using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaleManagerApi.Models;

[Table("khohang")]
public class Khohang
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string TenKho { get; set; } = string.Empty;

    public bool IsClosed { get; set; } = false;
}
