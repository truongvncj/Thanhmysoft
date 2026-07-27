using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SaleManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class AddSanphamnuocTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sanphamnuoc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KhohangId = table.Column<int>(type: "integer", nullable: false),
                    MaSanPham = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    TenSanPham = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    DinhLuong = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sanphamnuoc", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sanphamnuoc_khohang_KhohangId",
                        column: x => x.KhohangId,
                        principalTable: "khohang",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_sanphamnuoc_KhohangId",
                table: "sanphamnuoc",
                column: "KhohangId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sanphamnuoc");
        }
    }
}
