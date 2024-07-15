using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BadmintonBookingSystem.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _140720240125 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingOrder_TimeSlot_TimeSlotId",
                table: "BookingOrder");

            migrationBuilder.DropIndex(
                name: "IX_BookingOrder_TimeSlotId",
                table: "BookingOrder");

            migrationBuilder.RenameColumn(
                name: "TimeSlotId",
                table: "BookingOrder",
                newName: "TimeSlot");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeSlot",
                table: "BookingOrder",
                newName: "TimeSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingOrder_TimeSlotId",
                table: "BookingOrder",
                column: "TimeSlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingOrder_TimeSlot_TimeSlotId",
                table: "BookingOrder",
                column: "TimeSlotId",
                principalTable: "TimeSlot",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
