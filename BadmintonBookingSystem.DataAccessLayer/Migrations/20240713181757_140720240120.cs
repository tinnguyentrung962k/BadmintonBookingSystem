using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BadmintonBookingSystem.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _140720240120 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "TimeSlot");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "TimeSlot");

            migrationBuilder.AddColumn<string>(
                name: "TimeFrame",
                table: "TimeSlot",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeFrame",
                table: "TimeSlot");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "EndTime",
                table: "TimeSlot",
                type: "time without time zone",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "StartTime",
                table: "TimeSlot",
                type: "time without time zone",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));
        }
    }
}
