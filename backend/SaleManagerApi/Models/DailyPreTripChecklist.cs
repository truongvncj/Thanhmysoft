using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaleManagerApi.Models;

[Table("dailypretripchecklist")]
public class DailyPreTripChecklist
{
    [Key]
    public Guid Id { get; set; }
    
    [Column("date")]
    public DateTime Date { get; set; }
    
    [Column("malx")]
    public string MaLx { get; set; } = string.Empty;
    
    [Column("tenlx")]
    public string TenLx { get; set; } = string.Empty;
    
    [Column("sothe")]
    public string Sothe { get; set; } = string.Empty;
    
    [Column("manvt")]
    public string MaNvt { get; set; } = string.Empty;
    
    [Column("tennvt")]
    public string TenNvt { get; set; } = string.Empty;
    
    // JSON string storing the results: [{ ParamId: 1, IsPassed: true }, ...]
    [Column("checklistdata")]
    public string ChecklistData { get; set; } = string.Empty;
}
