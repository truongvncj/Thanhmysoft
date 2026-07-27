using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaleManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class AddDangTaiIdToChungTuVaoKho : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "dangtaiid",
                table: "chungtuvaokho",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dangtaiid",
                table: "chungtuvaokho");
        }
    }
}
