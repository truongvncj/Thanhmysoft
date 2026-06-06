using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SaleManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class HygieneCheck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dailykiemtravesinhxe",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tennhathau = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    tenlaixe = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ngaykiemtra = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    lanvaokhotrongngay = table.Column<int>(type: "integer", nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dailykiemtravesinhxe", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "kiemtravesinhxeparam",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    noidung = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    stt = table.Column<int>(type: "integer", nullable: false),
                    chitiet = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ghichu = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    dapandat = table.Column<bool>(type: "boolean", nullable: false),
                    isactive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kiemtravesinhxeparam", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dailykiemtravesinhxedetail",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    dailykiemtravesinhxeid = table.Column<int>(type: "integer", nullable: false),
                    paramid = table.Column<int>(type: "integer", nullable: false),
                    ketqua = table.Column<bool>(type: "boolean", nullable: false),
                    ghichu = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dailykiemtravesinhxedetail", x => x.id);
                    table.ForeignKey(
                        name: "FK_dailykiemtravesinhxedetail_dailykiemtravesinhxe_dailykiemtr~",
                        column: x => x.dailykiemtravesinhxeid,
                        principalTable: "dailykiemtravesinhxe",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dailykiemtravesinhxedetail_kiemtravesinhxeparam_paramid",
                        column: x => x.paramid,
                        principalTable: "kiemtravesinhxeparam",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dailykiemtravesinhxedetail_dailykiemtravesinhxeid",
                table: "dailykiemtravesinhxedetail",
                column: "dailykiemtravesinhxeid");

            migrationBuilder.CreateIndex(
                name: "IX_dailykiemtravesinhxedetail_paramid",
                table: "dailykiemtravesinhxedetail",
                column: "paramid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dailykiemtravesinhxedetail");

            migrationBuilder.DropTable(
                name: "dailykiemtravesinhxe");

            migrationBuilder.DropTable(
                name: "kiemtravesinhxeparam");
        }
    }
}
