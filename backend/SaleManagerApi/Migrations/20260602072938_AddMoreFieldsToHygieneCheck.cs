using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaleManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreFieldsToHygieneCheck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "malx",
                table: "dailykiemtravesinhxe",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "manvt",
                table: "dailykiemtravesinhxe",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "sothe",
                table: "dailykiemtravesinhxe",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "malx",
                table: "dailykiemtravesinhxe");

            migrationBuilder.DropColumn(
                name: "manvt",
                table: "dailykiemtravesinhxe");

            migrationBuilder.DropColumn(
                name: "sothe",
                table: "dailykiemtravesinhxe");
        }
    }
}
