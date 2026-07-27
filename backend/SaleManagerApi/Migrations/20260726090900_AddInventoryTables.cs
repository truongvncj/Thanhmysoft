using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SaleManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class AddInventoryTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "phatsinhsanpham",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaSanPham = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    TenSanPham = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    HanSuDung = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SoLuongChan = table.Column<int>(type: "integer", nullable: true),
                    SoLuongLe = table.Column<int>(type: "integer", nullable: true),
                    ViTri = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    LoaiPhatSinh = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    User = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    TenNhanVien = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ThoiGian = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Sothe = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    SoSTO = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_phatsinhsanpham", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tonkhohientai",
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
                    SttDayHang = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ThoiDiemGhiNhan = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tonkhohientai", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tonkhohientai_khohang_KhohangId",
                        column: x => x.KhohangId,
                        principalTable: "khohang",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tonkhohientai_KhohangId",
                table: "tonkhohientai",
                column: "KhohangId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "phatsinhsanpham");

            migrationBuilder.DropTable(
                name: "tonkhohientai");
        }
    }
}
