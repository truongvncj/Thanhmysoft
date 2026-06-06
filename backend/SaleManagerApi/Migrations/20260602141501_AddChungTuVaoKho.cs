using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaleManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class AddChungTuVaoKho : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DaDangKyChungTu",
                table: "DangTais",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "chungtuvaokho",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    sothe = table.Column<string>(type: "text", nullable: false),
                    khohangid = table.Column<int>(type: "integer", nullable: true),
                    ngaydangky = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    lydo = table.Column<string>(type: "text", nullable: false),
                    sotransferout = table.Column<string>(type: "text", nullable: true),
                    sosto = table.Column<string>(type: "text", nullable: true),
                    soshipment = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chungtuvaokho", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chungtuvaokho");

            migrationBuilder.DropColumn(
                name: "DaDangKyChungTu",
                table: "DangTais");
        }
    }
}
