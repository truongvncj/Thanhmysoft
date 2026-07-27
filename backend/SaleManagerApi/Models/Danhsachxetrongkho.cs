using System.ComponentModel.DataAnnotations;

namespace SaleManagerApi.Models;

public class Danhsachxetrongkho
{
    [Key]
    public Guid Id { get; set; }
    
    public string TenLaiXe { get; set; } = string.Empty;
    public string MaNhanVienLaiXe { get; set; } = string.Empty; // Mã Lx
    public string TenNhaThauVanTai { get; set; } = string.Empty;
    public string Sothe { get; set; } = string.Empty;
    public string BienSo { get; set; } = string.Empty;
    
    public DateTime GioDangKy { get; set; }
    public string LyDoVaoKho { get; set; } = string.Empty;
    public int? KhohangId { get; set; }
    
    public DateTime? Pre_Tripchecklist_Time { get; set; }
    public DateTime? KiemTraVeSinh_Time { get; set; }
    
    public DateTime? XacNhanVaoCong_Time { get; set; }
    public DateTime? ThuKhoXacNhan_Time { get; set; }
    public DateTime? XuatKho_Time { get; set; }
    public DateTime? NhapKho_Time { get; set; }
    public DateTime? XacNhanRaCong_Time { get; set; }
    public DateTime? BaoVeKiemTra_Time { get; set; }
    public DateTime? BaoVeKiemTra_PreTrip_Time { get; set; }
    public DateTime? BaoVeKiemTraTrongKho_Time { get; set; }
    
    // TrangThai: 0: Đã đăng ký, 1: Trong sân, 2: Chuẩn bị ra sân, 3: Đã hoàn thành
    public int TrangThai { get; set; } = 0; 
    
    public string? LyDoHuy { get; set; }
}
