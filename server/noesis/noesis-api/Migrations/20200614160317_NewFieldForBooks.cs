using Microsoft.EntityFrameworkCore.Migrations;

namespace noesis_api.Migrations
{
    public partial class NewFieldForBooks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThumbnailURL",
                table: "Book",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbnailURL",
                table: "Book");
        }
    }
}
