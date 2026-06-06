using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaleManagerApi.Models
{
    [Table("dailykiemtravesinhxe")]
    public class DailyKiemtravesinhxe
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("tennhathau")]
        [StringLength(255)]
        public string? TenNhaThau { get; set; }

        [Column("tenlaixe")]
        [StringLength(255)]
        public string? TenLaiXe { get; set; }

        [Column("malx")]
        [StringLength(50)]
        public string? MaLx { get; set; }

        [Column("manvt")]
        [StringLength(50)]
        public string? MaNvt { get; set; }

        [Column("sothe")]
        [StringLength(50)]
        public string? Sothe { get; set; }

        [Column("ngaykiemtra")]
        public DateTime NgayKiemTra { get; set; }

        [Column("lanvaokhotrongngay")]
        public int LanVaoKhoTrongNgay { get; set; }

        [Column("createdat")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<DailyKiemtravesinhxeDetail> Details { get; set; } = new List<DailyKiemtravesinhxeDetail>();
    }
}
