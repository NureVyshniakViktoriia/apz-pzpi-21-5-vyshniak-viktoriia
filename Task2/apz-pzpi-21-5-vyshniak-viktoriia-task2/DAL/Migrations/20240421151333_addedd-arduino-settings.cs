using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class addeddarduinosettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArduinoSettings",
                columns: table => new
                {
                    PetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemperatureMonitorIpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GPSTrackerIpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArduinoSettings", x => x.PetId);
                    table.ForeignKey(
                        name: "FK_ArduinoSettings_Pets_PetId",
                        column: x => x.PetId,
                        principalTable: "Pets",
                        principalColumn: "PetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RFIDSettings",
                columns: table => new
                {
                    InstitutionId = table.Column<int>(type: "int", nullable: false),
                    RFIDReaderIpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RFIDSettings", x => x.InstitutionId);
                    table.ForeignKey(
                        name: "FK_RFIDSettings_Institutions_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "Institutions",
                        principalColumn: "InstitutionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "UserProfiles",
                keyColumn: "UserId",
                keyValue: 1,
                column: "Email",
                value: "admin@pawpoint.com");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "RegisteredOnUtc",
                value: new DateTime(2024, 4, 21, 15, 13, 33, 203, DateTimeKind.Utc).AddTicks(6451));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArduinoSettings");

            migrationBuilder.DropTable(
                name: "RFIDSettings");

            migrationBuilder.UpdateData(
                table: "UserProfiles",
                keyColumn: "UserId",
                keyValue: 1,
                column: "Email",
                value: "admin@restogen.com");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "RegisteredOnUtc",
                value: new DateTime(2024, 4, 14, 11, 8, 25, 464, DateTimeKind.Utc).AddTicks(998));
        }
    }
}
