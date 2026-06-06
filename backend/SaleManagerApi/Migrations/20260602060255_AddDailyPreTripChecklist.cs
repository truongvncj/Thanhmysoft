using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaleManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class AddDailyPreTripChecklist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dailypretripchecklist",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    malx = table.Column<string>(type: "text", nullable: false),
                    sothe = table.Column<string>(type: "text", nullable: false),
                    manvt = table.Column<string>(type: "text", nullable: false),
                    tennvt = table.Column<string>(type: "text", nullable: false),
                    checklistdata = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dailypretripchecklist", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dailypretripchecklist");
        }
    }
}
