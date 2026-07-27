using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaleManagerApi.Models;

[Table("layoutkho")]
public class Layoutkho
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Column("khohang_id")]
    public int KhohangId { get; set; }

    [Required]
    [Column("ma_local")]
    [MaxLength(255)]
    public string MaLocal { get; set; } = string.Empty;

    [Column("hang")]
    public string Hang { get; set; } = string.Empty;

    [Column("cot")]
    public string Cot { get; set; } = string.Empty;

    [Column("ghi_chu")]
    public string GhiChu { get; set; } = string.Empty;

    [Column("position_x")]
    public double PositionX { get; set; } = 0;

    [Column("position_y")]
    public double PositionY { get; set; } = 0;

    [Column("width")]
    public double Width { get; set; } = 100;

    [Column("height")]
    public double Height { get; set; } = 50;

    [Column("element_type")]
    public string ElementType { get; set; } = "local";

    [Column("border_radius")]
    public double BorderRadius { get; set; } = 0;

    [Column("rotation")]
    public double Rotation { get; set; } = 0;

    [ForeignKey("KhohangId")]
    public virtual Khohang? Khohang { get; set; }
}
