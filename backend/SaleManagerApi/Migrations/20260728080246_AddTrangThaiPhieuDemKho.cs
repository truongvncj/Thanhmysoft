using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaleManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class AddTrangThaiPhieuDemKho : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NguoiDuyet",
                table: "PhieuDemKhos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ThoiGianDuyet",
                table: "PhieuDemKhos",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrangThai",
                table: "PhieuDemKhos",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NguoiDuyet",
                table: "PhieuDemKhos");

            migrationBuilder.DropColumn(
                name: "ThoiGianDuyet",
                table: "PhieuDemKhos");

            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "PhieuDemKhos");
        }
    }
}
