using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BadmintonBookingSystem.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _080720240245 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BadmintonCenterImage_BadmintonCenter_BadmintonCenterId",
                table: "BadmintonCenterImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BadmintonCenterImage",
                table: "BadmintonCenterImage");

            migrationBuilder.RenameTable(
                name: "BadmintonCenterImage",
                newName: "BadmintonCenterImages");

            migrationBuilder.RenameIndex(
                name: "IX_BadmintonCenterImage_BadmintonCenterId",
                table: "BadmintonCenterImages",
                newName: "IX_BadmintonCenterImages_BadmintonCenterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BadmintonCenterImages",
                table: "BadmintonCenterImages",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CourtImages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourtId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ImageLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastUpdatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourtImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourtImages_Court_CourtId",
                        column: x => x.CourtId,
                        principalTable: "Court",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourtImages_CourtId",
                table: "CourtImages",
                column: "CourtId");

            migrationBuilder.AddForeignKey(
                name: "FK_BadmintonCenterImages_BadmintonCenter_BadmintonCenterId",
                table: "BadmintonCenterImages",
                column: "BadmintonCenterId",
                principalTable: "BadmintonCenter",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BadmintonCenterImages_BadmintonCenter_BadmintonCenterId",
                table: "BadmintonCenterImages");

            migrationBuilder.DropTable(
                name: "CourtImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BadmintonCenterImages",
                table: "BadmintonCenterImages");

            migrationBuilder.RenameTable(
                name: "BadmintonCenterImages",
                newName: "BadmintonCenterImage");

            migrationBuilder.RenameIndex(
                name: "IX_BadmintonCenterImages_BadmintonCenterId",
                table: "BadmintonCenterImage",
                newName: "IX_BadmintonCenterImage_BadmintonCenterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BadmintonCenterImage",
                table: "BadmintonCenterImage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BadmintonCenterImage_BadmintonCenter_BadmintonCenterId",
                table: "BadmintonCenterImage",
                column: "BadmintonCenterId",
                principalTable: "BadmintonCenter",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
