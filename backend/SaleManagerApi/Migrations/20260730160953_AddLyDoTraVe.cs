using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaleManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class AddLyDoTraVe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "lydotrave",
                table: "chungtuvaokho",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "makhachhang",
                table: "chungtuvaokho",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "lydotrave",
                table: "chungtuvaokho");

            migrationBuilder.DropColumn(
                name: "makhachhang",
                table: "chungtuvaokho");
        }
    }
}
