using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SaleManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class AddLayoutkho : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "layoutkho",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    khohang_id = table.Column<int>(type: "integer", nullable: false),
                    ma_local = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    hang = table.Column<string>(type: "text", nullable: false),
                    cot = table.Column<string>(type: "text", nullable: false),
                    ghi_chu = table.Column<string>(type: "text", nullable: false),
                    position_x = table.Column<double>(type: "double precision", nullable: false),
                    position_y = table.Column<double>(type: "double precision", nullable: false),
                    width = table.Column<double>(type: "double precision", nullable: false),
                    height = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_layoutkho", x => x.id);
                    table.ForeignKey(
                        name: "FK_layoutkho_khohang_khohang_id",
                        column: x => x.khohang_id,
                        principalTable: "khohang",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_layoutkho_khohang_id",
                table: "layoutkho",
                column: "khohang_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "layoutkho");
        }
    }
}
