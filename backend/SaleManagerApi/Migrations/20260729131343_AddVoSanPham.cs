using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SaleManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class AddVoSanPham : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "sanphamnuoc",
                newName: "sanpham");

            migrationBuilder.AddColumn<string>(
                name: "LoaiSanPham",
                table: "sanpham",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Thành phẩm");

            migrationBuilder.AddColumn<string>(
                name: "MaVo",
                table: "sanpham",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenVo",
                table: "sanpham",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoaiSanPham",
                table: "sanpham");

            migrationBuilder.DropColumn(
                name: "MaVo",
                table: "sanpham");

            migrationBuilder.DropColumn(
                name: "TenVo",
                table: "sanpham");

            migrationBuilder.RenameTable(
                name: "sanpham",
                newName: "sanphamnuoc");
        }
    }
}
