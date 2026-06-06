using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SaleManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateKhohangAndDangtai : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "password",
                table: "nhanvien",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KhohangId",
                table: "DangTais",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LyDo",
                table: "DangTais",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TrangThai",
                table: "DangTais",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "khohang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenKho = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    IsClosed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_khohang", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "khohang");

            migrationBuilder.DropColumn(
                name: "password",
                table: "nhanvien");

            migrationBuilder.DropColumn(
                name: "KhohangId",
                table: "DangTais");

            migrationBuilder.DropColumn(
                name: "LyDo",
                table: "DangTais");

            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "DangTais");
        }
    }
}
