using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaleManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class AddNhanvien : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "nhanvien",
                columns: table => new
                {
                    mnv = table.Column<string>(type: "text", nullable: false),
                    tnv = table.Column<string>(type: "text", nullable: false),
                    chucdanh = table.Column<string>(type: "text", nullable: false),
                    vitri = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nhanvien", x => x.mnv);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "nhanvien");
        }
    }
}
