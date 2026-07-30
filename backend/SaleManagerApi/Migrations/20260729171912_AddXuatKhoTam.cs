using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SaleManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class AddXuatKhoTam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "xuatkhotam",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaNhanVien = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SoChungTu = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    MaSanPham = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    TenSanPham = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ViTri = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    SoLuongChan = table.Column<int>(type: "integer", nullable: false),
                    SoLuongLe = table.Column<int>(type: "integer", nullable: false),
                    NgaySanXuat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    HanSuDung = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    KhohangId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_xuatkhotam", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "xuatkhotam");
        }
    }
}
