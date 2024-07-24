using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BadmintonBookingSystem.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _240720241148 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "TimeSlot",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isActive",
                table: "TimeSlot");
        }
    }
}
