using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SaleManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class AddTonKhoDauKy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tonkhodauky",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KhohangId = table.Column<int>(type: "integer", nullable: false),
                    ViTri = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    MaHang = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    TenSanPham = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    HanSuDung = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PhanTramHanSuDung = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    SoLuongPalletChan = table.Column<int>(type: "integer", nullable: true),
                    SoThungLe = table.Column<int>(type: "integer", nullable: true),
                    DinhLuong = table.Column<int>(type: "integer", nullable: true),
                    Tong = table.Column<int>(type: "integer", nullable: true),
                    GhiChu = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    TrangThaiDOD = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    NgaySanXuat = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DayHang = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    SttDayHang = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tonkhodauky", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tonkhodauky_khohang_KhohangId",
                        column: x => x.KhohangId,
                        principalTable: "khohang",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tonkhodauky_KhohangId",
                table: "tonkhodauky",
                column: "KhohangId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tonkhodauky");
        }
    }
}
