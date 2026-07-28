using System;
using System.Collections.Generic;

namespace SaleManagerApi.Models
{
    public class PhieuDemKho
    {
        public int Id { get; set; }
        public int KhohangId { get; set; }
        public string ViTri { get; set; } = string.Empty;
        public DateTime ThoiGianTao { get; set; } = DateTime.UtcNow.AddHours(7);
        public string NguoiDem { get; set; } = string.Empty;
        public string GhiChu { get; set; } = string.Empty;
        
        public string TrangThai { get; set; } = "Chờ duyệt"; // Chờ duyệt, Duyệt, Hủy
        public string NguoiDuyet { get; set; } = string.Empty;
        public DateTime? ThoiGianDuyet { get; set; }
        
        // Navigation properties
        public Khohang? Khohang { get; set; }
        public ICollection<ChiTietDemKho> ChiTiets { get; set; } = new List<ChiTietDemKho>();
    }
}
