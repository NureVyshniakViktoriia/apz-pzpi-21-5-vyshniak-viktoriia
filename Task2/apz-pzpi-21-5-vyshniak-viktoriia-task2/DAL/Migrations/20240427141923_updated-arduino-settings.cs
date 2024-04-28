using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class updatedarduinosettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GPSTrackerIpAddress",
                table: "ArduinoSettings");

            migrationBuilder.RenameColumn(
                name: "TemperatureMonitorIpAddress",
                table: "ArduinoSettings",
                newName: "PetDeviceIpAddress");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "RegisteredOnUtc",
                value: new DateTime(2024, 4, 27, 14, 19, 22, 801, DateTimeKind.Utc).AddTicks(4888));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PetDeviceIpAddress",
                table: "ArduinoSettings",
                newName: "TemperatureMonitorIpAddress");

            migrationBuilder.AddColumn<string>(
                name: "GPSTrackerIpAddress",
                table: "ArduinoSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "RegisteredOnUtc",
                value: new DateTime(2024, 4, 21, 15, 13, 33, 203, DateTimeKind.Utc).AddTicks(6451));
        }
    }
}
