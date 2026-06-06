using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaleManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedPreTripChecklistParam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "pretripchecklistparam",
                columns: new[] { "id", "dieukien", "isactive", "noidung", "stt" },
                values: new object[] { 1, "Bằng lái xe phù hợp với loại xe điều khiển và còn hạn...", true, "Hồ sơ và Nhân viên lái xe", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "pretripchecklistparam",
                keyColumn: "id",
                keyValue: 1);
        }
    }
}
