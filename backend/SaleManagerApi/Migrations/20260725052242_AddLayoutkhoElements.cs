using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaleManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class AddLayoutkhoElements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "border_radius",
                table: "layoutkho",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "element_type",
                table: "layoutkho",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "rotation",
                table: "layoutkho",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "border_radius",
                table: "layoutkho");

            migrationBuilder.DropColumn(
                name: "element_type",
                table: "layoutkho");

            migrationBuilder.DropColumn(
                name: "rotation",
                table: "layoutkho");
        }
    }
}
