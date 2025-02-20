using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserPasswords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ApiUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "JennyPassword12!");

            migrationBuilder.UpdateData(
                table: "ApiUsers",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "WatermelonPassword3e?");

            migrationBuilder.UpdateData(
                table: "ApiUsers",
                keyColumn: "Id",
                keyValue: 3,
                column: "Password",
                value: "JinPassword!23");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ApiUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "JennyPassword");

            migrationBuilder.UpdateData(
                table: "ApiUsers",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "WatermelonPassword");

            migrationBuilder.UpdateData(
                table: "ApiUsers",
                keyColumn: "Id",
                keyValue: 3,
                column: "Password",
                value: "JinPassword");
        }
    }
}
