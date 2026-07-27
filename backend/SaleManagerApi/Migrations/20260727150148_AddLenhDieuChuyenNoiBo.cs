using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SaleManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class AddLenhDieuChuyenNoiBo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LenhDieuChuyenNoiBos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaLenh = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    KhohangId = table.Column<int>(type: "integer", nullable: false),
                    ThoiGianTao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NguoiTao = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TrangThai = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ThoiGianHoanThanh = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    NguoiHoanThanh = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LenhDieuChuyenNoiBos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LenhDieuChuyenNoiBos_khohang_KhohangId",
                        column: x => x.KhohangId,
                        principalTable: "khohang",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDieuChuyenNoiBos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LenhDieuChuyenNoiBoId = table.Column<int>(type: "integer", nullable: false),
                    MaSanPham = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TenSanPham = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    NgaySanXuat = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    HanSuDung = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ViTriDi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ViTriDen = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SoLuongChan = table.Column<int>(type: "integer", nullable: false),
                    SoLuongLe = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietDieuChuyenNoiBos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChiTietDieuChuyenNoiBos_LenhDieuChuyenNoiBos_LenhDieuChuyen~",
                        column: x => x.LenhDieuChuyenNoiBoId,
                        principalTable: "LenhDieuChuyenNoiBos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDieuChuyenNoiBos_LenhDieuChuyenNoiBoId",
                table: "ChiTietDieuChuyenNoiBos",
                column: "LenhDieuChuyenNoiBoId");

            migrationBuilder.CreateIndex(
                name: "IX_LenhDieuChuyenNoiBos_KhohangId",
                table: "LenhDieuChuyenNoiBos",
                column: "KhohangId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietDieuChuyenNoiBos");

            migrationBuilder.DropTable(
                name: "LenhDieuChuyenNoiBos");
        }
    }
}
