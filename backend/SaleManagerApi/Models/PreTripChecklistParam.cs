using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaleManagerApi.Models;

[Table("pretripchecklistparam")]
public class PreTripChecklistParam
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Column("noidung")]
    [MaxLength(255)]
    public string NoiDung { get; set; } = string.Empty;

    [Column("stt")]
    public int Stt { get; set; }

    [Column("dieukien")]
    [MaxLength(1000)]
    public string DieuKien { get; set; } = string.Empty;
    
    [Column("isactive")]
    public bool IsActive { get; set; } = true;
}
