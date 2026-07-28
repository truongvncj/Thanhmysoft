using System;

namespace SaleManagerApi.Models
{
    public class ChiTietDemKho
    {
        public int Id { get; set; }
        public int PhieuDemKhoId { get; set; }
        
        public string MaSanPham { get; set; } = string.Empty;
        public string TenSanPham { get; set; } = string.Empty;
        public DateTime? NgaySanXuat { get; set; }
        public DateTime? HanSuDung { get; set; }
        
        // Tồn hiện tại
        public int TonChanHienTai { get; set; }
        public int TonLeHienTai { get; set; }
        
        // Số đếm thực tế
        public int SoDemChan { get; set; }
        public int SoDemLe { get; set; }
        
        // Chênh lệch
        public int ChenhLechChan { get; set; }
        public int ChenhLechLe { get; set; }

        public string GhiChu { get; set; } = string.Empty;
        
        // Navigation properties
        public PhieuDemKho? PhieuDemKho { get; set; }
    }
}
