using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BadmintonBookingSystem.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _21052024323 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingOrder_Slot_SlotId",
                table: "BookingOrder");

            migrationBuilder.DropTable(
                name: "Slot");

            migrationBuilder.RenameColumn(
                name: "SlotId",
                table: "BookingOrder",
                newName: "CourtId");

            migrationBuilder.RenameIndex(
                name: "IX_BookingOrder_SlotId",
                table: "BookingOrder",
                newName: "IX_BookingOrder_CourtId");

            migrationBuilder.CreateTable(
                name: "Court",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourtName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CenterId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastUpdatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Court", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Court_BadmintonCenter_CenterId",
                        column: x => x.CenterId,
                        principalTable: "BadmintonCenter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Court_CenterId",
                table: "Court",
                column: "CenterId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingOrder_Court_CourtId",
                table: "BookingOrder",
                column: "CourtId",
                principalTable: "Court",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingOrder_Court_CourtId",
                table: "BookingOrder");

            migrationBuilder.DropTable(
                name: "Court");

            migrationBuilder.RenameColumn(
                name: "CourtId",
                table: "BookingOrder",
                newName: "SlotId");

            migrationBuilder.RenameIndex(
                name: "IX_BookingOrder_CourtId",
                table: "BookingOrder",
                newName: "IX_BookingOrder_SlotId");

            migrationBuilder.CreateTable(
                name: "Slot",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CenterId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastUpdatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    SlotName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Slot_BadmintonCenter_CenterId",
                        column: x => x.CenterId,
                        principalTable: "BadmintonCenter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Slot_CenterId",
                table: "Slot",
                column: "CenterId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingOrder_Slot_SlotId",
                table: "BookingOrder",
                column: "SlotId",
                principalTable: "Slot",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
