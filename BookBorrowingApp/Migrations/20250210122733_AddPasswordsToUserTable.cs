using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordsToUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "ApiUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "ApiUsers");
        }
    }
}
