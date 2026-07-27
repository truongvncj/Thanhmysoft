using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaleManagerApi.Models;

[Table("chungtuvaokho")]
public class ChungTuVaoKho
{
    [Key]
    public Guid Id { get; set; }
    
    [Column("sothe")]
    public string Sothe { get; set; } = string.Empty;
    
    [Column("khohangid")]
    public int? KhohangId { get; set; }
    
    [Column("ngaydangky")]
    public DateTime NgayDangKy { get; set; }
    
    [Column("dangtaiid")]
    public Guid? DangTaiId { get; set; }
    
    [Column("lydo")]
    public string LyDo { get; set; } = string.Empty; // Nhập hàng / Xuất hàng
    
    [Column("sotransferout")]
    public string? SoTransferOut { get; set; }
    
    [Column("sosto")]
    public string? SoSTO { get; set; }
    
    [Column("soshipment")]
    public string? SoShipment { get; set; }
}
