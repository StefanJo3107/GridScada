using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AlarmAlert",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AlarmId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Value = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlarmAlert", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnalogInput",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    ScanTime = table.Column<int>(type: "INTEGER", nullable: false),
                    ScanOn = table.Column<bool>(type: "INTEGER", nullable: false),
                    LowLimit = table.Column<double>(type: "REAL", nullable: false),
                    HighLimit = table.Column<double>(type: "REAL", nullable: false),
                    Unit = table.Column<string>(type: "TEXT", nullable: false),
                    IOAddress = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalogInput", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DigitalInput",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    ScanTime = table.Column<int>(type: "INTEGER", nullable: false),
                    ScanOn = table.Column<bool>(type: "INTEGER", nullable: false),
                    IOAddress = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DigitalInput", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Surname = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedById = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Alarm",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    EdgeValue = table.Column<double>(type: "REAL", nullable: false),
                    Unit = table.Column<string>(type: "TEXT", nullable: false),
                    AnalogInputId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alarm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alarm_AnalogInput_AnalogInputId",
                        column: x => x.AnalogInputId,
                        principalTable: "AnalogInput",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnalogData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AnalogInputId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Value = table.Column<double>(type: "REAL", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalogData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnalogData_AnalogInput_AnalogInputId",
                        column: x => x.AnalogInputId,
                        principalTable: "AnalogInput",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DigitalData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    DigitalInputId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Value = table.Column<double>(type: "REAL", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DigitalData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DigitalData_DigitalInput_DigitalInputId",
                        column: x => x.DigitalInputId,
                        principalTable: "DigitalInput",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AnalogInputUser",
                columns: table => new
                {
                    AnalogInputsId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UsersId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalogInputUser", x => new { x.AnalogInputsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_AnalogInputUser_AnalogInput_AnalogInputsId",
                        column: x => x.AnalogInputsId,
                        principalTable: "AnalogInput",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnalogInputUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DigitalInputUser",
                columns: table => new
                {
                    DigitalInputsId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UsersId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DigitalInputUser", x => new { x.DigitalInputsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_DigitalInputUser_DigitalInput_DigitalInputsId",
                        column: x => x.DigitalInputsId,
                        principalTable: "DigitalInput",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DigitalInputUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alarm_AnalogInputId",
                table: "Alarm",
                column: "AnalogInputId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalogData_AnalogInputId",
                table: "AnalogData",
                column: "AnalogInputId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalogInputUser_UsersId",
                table: "AnalogInputUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_DigitalData_DigitalInputId",
                table: "DigitalData",
                column: "DigitalInputId");

            migrationBuilder.CreateIndex(
                name: "IX_DigitalInputUser_UsersId",
                table: "DigitalInputUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedById",
                table: "Users",
                column: "CreatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alarm");

            migrationBuilder.DropTable(
                name: "AlarmAlert");

            migrationBuilder.DropTable(
                name: "AnalogData");

            migrationBuilder.DropTable(
                name: "AnalogInputUser");

            migrationBuilder.DropTable(
                name: "DigitalData");

            migrationBuilder.DropTable(
                name: "DigitalInputUser");

            migrationBuilder.DropTable(
                name: "AnalogInput");

            migrationBuilder.DropTable(
                name: "DigitalInput");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
