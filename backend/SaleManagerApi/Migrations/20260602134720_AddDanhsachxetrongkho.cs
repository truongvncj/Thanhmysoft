using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaleManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class AddDanhsachxetrongkho : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Danhsachxetrongkhos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenLaiXe = table.Column<string>(type: "text", nullable: false),
                    MaNhanVienLaiXe = table.Column<string>(type: "text", nullable: false),
                    TenNhaThauVanTai = table.Column<string>(type: "text", nullable: false),
                    Sothe = table.Column<string>(type: "text", nullable: false),
                    BienSo = table.Column<string>(type: "text", nullable: false),
                    GioDangKy = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LyDoVaoKho = table.Column<string>(type: "text", nullable: false),
                    KhohangId = table.Column<int>(type: "integer", nullable: true),
                    Pre_Tripchecklist_Time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    KiemTraVeSinh_Time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    XacNhanVaoCong_Time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ThuKhoXacNhan_Time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    XacNhanRaCong_Time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TrangThai = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Danhsachxetrongkhos", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Danhsachxetrongkhos");
        }
    }
}
