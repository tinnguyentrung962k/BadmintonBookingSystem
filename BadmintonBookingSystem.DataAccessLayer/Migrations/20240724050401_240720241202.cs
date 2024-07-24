using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BadmintonBookingSystem.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _240720241202 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isActive",
                table: "TimeSlot",
                newName: "IsActive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "TimeSlot",
                newName: "isActive");
        }
    }
}
