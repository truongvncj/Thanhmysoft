using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaleManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class AddLyDoHuy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LyDoHuy",
                table: "Danhsachxetrongkhos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LyDoHuy",
                table: "DangTais",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LyDoHuy",
                table: "Danhsachxetrongkhos");

            migrationBuilder.DropColumn(
                name: "LyDoHuy",
                table: "DangTais");
        }
    }
}
