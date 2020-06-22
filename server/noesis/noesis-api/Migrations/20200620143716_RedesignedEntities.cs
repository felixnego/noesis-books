using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace noesis_api.Migrations
{
    public partial class RedesignedEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Format_FormatId",
                table: "Book");

            migrationBuilder.DropTable(
                name: "Format");

            migrationBuilder.DropIndex(
                name: "IX_Book_FormatId",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "FormatId",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Author");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Author");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Author");

            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "Author");

            migrationBuilder.DropColumn(
                name: "WikiURL",
                table: "Author");

            migrationBuilder.AddColumn<string>(
                name: "CoverBigURL",
                table: "Book",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "GoodReadsId",
                table: "Book",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<double>(
                name: "Pages",
                table: "Book",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Publisher",
                table: "Book",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Book",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "About",
                table: "Author",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Author",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Author",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "PlaceOfBirth",
                table: "Author",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CategoryDescription = table.Column<string>(nullable: true),
                    BookId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Category_Book_BookId",
                        column: x => x.BookId,
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Category_BookId",
                table: "Category",
                column: "BookId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropColumn(
                name: "CoverBigURL",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "GoodReadsId",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "Pages",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "Publisher",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "About",
                table: "Author");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Author");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Author");

            migrationBuilder.DropColumn(
                name: "PlaceOfBirth",
                table: "Author");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Book",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FormatId",
                table: "Book",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Book",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Author",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Author",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Author",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "Author",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WikiURL",
                table: "Author",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Format",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FormatDescription = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Format", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Book_FormatId",
                table: "Book",
                column: "FormatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Format_FormatId",
                table: "Book",
                column: "FormatId",
                principalTable: "Format",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
