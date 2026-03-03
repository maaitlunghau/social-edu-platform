using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace server.Migrations
{
    /// <inheritdoc />
    public partial class SocialEduPlatformDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Role = table.Column<int>(type: "int", maxLength: 20, nullable: false),
                    Avatar = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsEmailVerified = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAtUTC = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAtUTC = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RefreshTokenRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RefreshToken = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccessTokenJTI = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReplacedByRefreshToken = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RevokedAtUTC = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ExpireAtUTC = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedAtUTC = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAtUTC = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokenRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokenRecords_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Avatar", "CreatedAtUTC", "Email", "IsEmailVerified", "Name", "Password", "Phone", "Role", "Status", "UpdatedAtUTC" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "trunghau@mstsoftware.vn", true, "Mai Trung Hau", "$2a$11$hujKbVuakeRCRTpkQOgFOuW7XucJlZIBk1KWXhVP7Vct0E3C44vDi", "", 2, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "john@example.com", true, "John Doe", "$2a$11$yDt/GUmudcv29HAmrnBKyOzvNXnTmlwvOaMCpwh8t3/wTmk1b331u", "", 0, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "jane@example.com", true, "Jane Smith", "$2a$11$4gEnSkWwdsI/.5baZtkShOfHNoc3LEsxm8uwBVcmGSzMHGhVj8opm", "", 1, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "bob@example.com", false, "Bob Johnson", "$2a$11$6jC9r6N1lnzvlQtswvZIT.yiGxwsNPRypqFhGJV5Y5.bTxO8scTh6", "", 0, 0, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "alice@example.com", true, "Alice Williams", "$2a$11$b479NmPvRLL1wjNy87QR8eO5262EZk6nUi4GEpjQLuYeWZAlaBYRa", "", 0, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokenRecords_AccessTokenJTI",
                table: "RefreshTokenRecords",
                column: "AccessTokenJTI");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokenRecords_RefreshToken",
                table: "RefreshTokenRecords",
                column: "RefreshToken");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokenRecords_UserId",
                table: "RefreshTokenRecords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokenRecords");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
