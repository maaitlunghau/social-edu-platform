using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    /// <inheritdoc />
    public partial class ConvertEnumToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Users",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 20)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "Password", "Role", "Status" },
                values: new object[] { "$2a$11$1qDM5fO34BNjI3jfrEG3c..X10A7Qtfz8mRrHarCmaMLNf5fyHJtu", "Admin", "Active" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "Password", "Role", "Status" },
                values: new object[] { "$2a$11$dXUXwXo4cxlXKTsG4AkKFeUcQVJvRZqTIGE98uvFhSpFg7175dnQK", "User", "Active" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "Password", "Role", "Status" },
                values: new object[] { "$2a$11$bte.dahFWsBbTkYzHjYap.pxhINJPIlfIHF3FwugwomUWr3zoKNhq", "Educator", "Active" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "Password", "Role", "Status" },
                values: new object[] { "$2a$11$M0O/fDR4pM2TMlX7DpAFTOSZUvAxhZno8HZQoBdMi09oEBx0BP5oa", "User", "Pending" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "Password", "Role", "Status" },
                values: new object[] { "$2a$11$z/QXzNeJkBY93BIv7Rk7ru9uR4sH3keQ5KvPyt.t.DVXtMLHbiJ9e", "User", "Banned" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "Role",
                table: "Users",
                type: "int",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "Password", "Role", "Status" },
                values: new object[] { "$2a$11$hujKbVuakeRCRTpkQOgFOuW7XucJlZIBk1KWXhVP7Vct0E3C44vDi", 2, 1 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "Password", "Role", "Status" },
                values: new object[] { "$2a$11$yDt/GUmudcv29HAmrnBKyOzvNXnTmlwvOaMCpwh8t3/wTmk1b331u", 0, 1 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "Password", "Role", "Status" },
                values: new object[] { "$2a$11$4gEnSkWwdsI/.5baZtkShOfHNoc3LEsxm8uwBVcmGSzMHGhVj8opm", 1, 1 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "Password", "Role", "Status" },
                values: new object[] { "$2a$11$6jC9r6N1lnzvlQtswvZIT.yiGxwsNPRypqFhGJV5Y5.bTxO8scTh6", 0, 0 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "Password", "Role", "Status" },
                values: new object[] { "$2a$11$b479NmPvRLL1wjNy87QR8eO5262EZk6nUi4GEpjQLuYeWZAlaBYRa", 0, 2 });
        }
    }
}
