using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SaleManagerApi.Models
{
    [Table("dailykiemtravesinhxedetail")]
    public class DailyKiemtravesinhxeDetail
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("dailykiemtravesinhxeid")]
        public int DailyKiemtravesinhxeId { get; set; }

        [Column("paramid")]
        public int ParamId { get; set; }

        [Column("ketqua")]
        public bool KetQua { get; set; } // true: Có, false: Không

        [Column("ghichu")]
        [StringLength(1000)]
        public string? GhiChu { get; set; }

        [JsonIgnore]
        public virtual DailyKiemtravesinhxe? DailyKiemtravesinhxe { get; set; }

        public virtual KiemtravesinhxeParam? Param { get; set; }
    }
}
