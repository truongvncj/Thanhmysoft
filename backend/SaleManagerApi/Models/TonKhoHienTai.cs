using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaleManagerApi.Models;

[Table("tonkhohientai")]
public class TonKhoHienTai
{
    [Key]
    public int Id { get; set; }

    public int KhohangId { get; set; }

    [ForeignKey("KhohangId")]
    public Khohang? Khohang { get; set; }

    [MaxLength(255)]
    public string? ViTri { get; set; }

    [MaxLength(255)]
    public string? MaHang { get; set; }

    [MaxLength(500)]
    public string? TenSanPham { get; set; }

    public DateTime? HanSuDung { get; set; }

    [MaxLength(255)]
    public string? PhanTramHanSuDung { get; set; }

    public int? SoLuongPalletChan { get; set; }

    public int? SoThungLe { get; set; }

    public int? DinhLuong { get; set; }

    public int? Tong { get; set; }

    [MaxLength(1000)]
    public string? GhiChu { get; set; }

    [MaxLength(255)]
    public string? TrangThaiDOD { get; set; }

    public DateTime? NgaySanXuat { get; set; }

    [MaxLength(255)]
    public string? DayHang { get; set; }

    [MaxLength(255)]
    public string? SttDayHang { get; set; }

    public DateTime? ThoiDiemGhiNhan { get; set; }

    [MaxLength(50)]
    public string? LoaiHang { get; set; } // "Thành phẩm", "Vỏ", "Hàng đổi 1-1", "Hàng trả về", "Hàng xuất nhầm"
}
