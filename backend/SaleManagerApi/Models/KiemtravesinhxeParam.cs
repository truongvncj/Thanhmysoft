using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaleManagerApi.Models
{
    [Table("kiemtravesinhxeparam")]
    public class KiemtravesinhxeParam
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("noidung")]
        [StringLength(255)]
        public string? NoiDung { get; set; }

        [Column("stt")]
        public int Stt { get; set; }

        [Column("chitiet")]
        [StringLength(1000)]
        public string? ChiTiet { get; set; }

        [Column("ghichu")]
        [StringLength(1000)]
        public string? GhiChu { get; set; }

        [Column("dapandat")]
        public bool DapAnDat { get; set; } // true: "Có" là đạt, false: "Không" là đạt

        [Column("isactive")]
        public bool IsActive { get; set; } = true;
    }
}
