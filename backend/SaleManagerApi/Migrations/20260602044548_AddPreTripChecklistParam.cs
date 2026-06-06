using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SaleManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class AddPreTripChecklistParam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pretripchecklistparam",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    noidung = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    stt = table.Column<int>(type: "integer", nullable: false),
                    dieukien = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    isactive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pretripchecklistparam", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pretripchecklistparam");
        }
    }
}
