using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BadmintonBookingSystem.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _150720240705 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingOrder");

            migrationBuilder.DropTable(
                name: "TimeSlot");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookingOrder",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CourtId = table.Column<string>(type: "text", nullable: false),
                    CustomerId = table.Column<string>(type: "text", nullable: false),
                    BookingDate = table.Column<DateOnly>(type: "date", nullable: false),
                    BookingType = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsCheckIn = table.Column<bool>(type: "boolean", nullable: false),
                    IsPaid = table.Column<bool>(type: "boolean", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "text", nullable: true),
                    LastUpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TimeSlot = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingOrder_Court_CourtId",
                        column: x => x.CourtId,
                        principalTable: "Court",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingOrder_User_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TimeSlot",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CourtId = table.Column<string>(type: "text", nullable: false),
                    BookingType = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "text", nullable: true),
                    LastUpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    TimeFrame = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSlot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeSlot_Court_CourtId",
                        column: x => x.CourtId,
                        principalTable: "Court",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingOrder_CourtId",
                table: "BookingOrder",
                column: "CourtId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingOrder_CustomerId",
                table: "BookingOrder",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeSlot_CourtId",
                table: "TimeSlot",
                column: "CourtId");
        }
    }
}
