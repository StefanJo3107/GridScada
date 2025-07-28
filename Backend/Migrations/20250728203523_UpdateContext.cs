using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnalogInputUser");

            migrationBuilder.DropTable(
                name: "DigitalInputUser");

            migrationBuilder.CreateTable(
                name: "UsersToAnalogInputsJoinTable",
                columns: table => new
                {
                    AnalogInputsId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UsersId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersToAnalogInputsJoinTable", x => new { x.AnalogInputsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_UsersToAnalogInputsJoinTable_AnalogInput_AnalogInputsId",
                        column: x => x.AnalogInputsId,
                        principalTable: "AnalogInput",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersToAnalogInputsJoinTable_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsersToDigitalInputsJoinTable",
                columns: table => new
                {
                    DigitalInputsId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UsersId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersToDigitalInputsJoinTable", x => new { x.DigitalInputsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_UsersToDigitalInputsJoinTable_DigitalInput_DigitalInputsId",
                        column: x => x.DigitalInputsId,
                        principalTable: "DigitalInput",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersToDigitalInputsJoinTable_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedById", "Email", "Name", "Password", "Role", "Surname" },
                values: new object[] { new Guid("898b1f78-ddfa-4f84-8349-3f8ec62f52bf"), null, "admin@example.com", "Admin", "$2a$12$G7/QkZt1iK9viHX6HYa73OtnmgF/xYeJOf8klJ9XjnoYBEkHAWZhu", "Admin", "Admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsersToAnalogInputsJoinTable_UsersId",
                table: "UsersToAnalogInputsJoinTable",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersToDigitalInputsJoinTable_UsersId",
                table: "UsersToDigitalInputsJoinTable",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersToAnalogInputsJoinTable");

            migrationBuilder.DropTable(
                name: "UsersToDigitalInputsJoinTable");

            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("898b1f78-ddfa-4f84-8349-3f8ec62f52bf"));

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
                name: "IX_AnalogInputUser_UsersId",
                table: "AnalogInputUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_DigitalInputUser_UsersId",
                table: "DigitalInputUser",
                column: "UsersId");
        }
    }
}
