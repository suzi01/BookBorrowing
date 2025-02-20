using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Publisher = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApiUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.BookId);
                    table.ForeignKey(
                        name: "FK_Books_ApiUsers_ApiUserId",
                        column: x => x.ApiUserId,
                        principalTable: "ApiUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "ApiUsers",
                columns: new[] { "Id", "Email", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, "JennyCrews@hotmail.com", "Jenny", "Crews" },
                    { 2, "watermelonSugar@hotmail.com", "Harry", "Styles" },
                    { 3, "tshimaGhost@hotmail.com", "Jin", "Sakai" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "BookId", "ApiUserId", "Author", "Publisher", "Title" },
                values: new object[,]
                {
                    { 3, null, "Richard Swan", "Tor Books", "Justice of Kings" },
                    { 4, null, "Christopher Rucchio", "Tor Books", "Empire of Silence" },
                    { 5, null, "Robert Jackson Bennet", "Tor Books", "Tainted Cup" },
                    { 1, 1, "Brandon Sanderson", "Tor Books", "The Final Empire" },
                    { 2, 1, "Brandon Sanderson", "Tor Books", "The Well Of Ascension" },
                    { 11, 2, "J.k.Rowling", "Bloomsbury", "Harry Potter and The Philosopher's stone " },
                    { 12, 2, "J.k.Rowling", "Bloomsbury", "Harry Potter and The Chambers of Secrets " },
                    { 13, 2, "J.k.Rowling", "Bloomsbury", "Harry Potter and The Prisoner of Azkaban " },
                    { 14, 3, "J.k.Rowling", "Bloomsbury", "Harry Potter and The Goblet of Fire " }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_ApiUserId",
                table: "Books",
                column: "ApiUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "ApiUsers");
        }
    }
}
