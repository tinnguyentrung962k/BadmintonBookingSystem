using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BadmintonBookingSystem.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _180720241502 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateOfWeek",
                table: "BookingDetail",
                newName: "DayOfAWeek");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DayOfAWeek",
                table: "BookingDetail",
                newName: "DateOfWeek");
        }
    }
}
