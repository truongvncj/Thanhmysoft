using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SaleManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class AddDemKho : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PhieuDemKhos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KhohangId = table.Column<int>(type: "integer", nullable: false),
                    ViTri = table.Column<string>(type: "text", nullable: false),
                    ThoiGianTao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    NguoiDem = table.Column<string>(type: "text", nullable: false),
                    GhiChu = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhieuDemKhos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhieuDemKhos_khohang_KhohangId",
                        column: x => x.KhohangId,
                        principalTable: "khohang",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDemKhos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PhieuDemKhoId = table.Column<int>(type: "integer", nullable: false),
                    MaSanPham = table.Column<string>(type: "text", nullable: false),
                    TenSanPham = table.Column<string>(type: "text", nullable: false),
                    NgaySanXuat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    HanSuDung = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    TonChanHienTai = table.Column<int>(type: "integer", nullable: false),
                    TonLeHienTai = table.Column<int>(type: "integer", nullable: false),
                    SoDemChan = table.Column<int>(type: "integer", nullable: false),
                    SoDemLe = table.Column<int>(type: "integer", nullable: false),
                    ChenhLechChan = table.Column<int>(type: "integer", nullable: false),
                    ChenhLechLe = table.Column<int>(type: "integer", nullable: false),
                    GhiChu = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietDemKhos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChiTietDemKhos_PhieuDemKhos_PhieuDemKhoId",
                        column: x => x.PhieuDemKhoId,
                        principalTable: "PhieuDemKhos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDemKhos_PhieuDemKhoId",
                table: "ChiTietDemKhos",
                column: "PhieuDemKhoId");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuDemKhos_KhohangId",
                table: "PhieuDemKhos",
                column: "KhohangId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietDemKhos");

            migrationBuilder.DropTable(
                name: "PhieuDemKhos");
        }
    }
}
