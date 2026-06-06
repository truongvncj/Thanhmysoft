using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SaleManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class AddDangTaiTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.CreateTable(
                name: "DangTais",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MaLx = table.Column<string>(type: "text", nullable: false),
                    Sothe = table.Column<string>(type: "text", nullable: false),
                    NgayDangTai = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DaDangTai = table.Column<bool>(type: "boolean", nullable: false),
                    DaVeSinh = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DangTais", x => x.Id);
                });


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DropTable(
                name: "DangTais");


        }
    }
}
