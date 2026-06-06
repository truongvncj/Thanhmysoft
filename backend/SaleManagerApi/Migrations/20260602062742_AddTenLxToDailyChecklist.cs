using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaleManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class AddTenLxToDailyChecklist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "tenlx",
                table: "dailypretripchecklist",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tenlx",
                table: "dailypretripchecklist");
        }
    }
}
