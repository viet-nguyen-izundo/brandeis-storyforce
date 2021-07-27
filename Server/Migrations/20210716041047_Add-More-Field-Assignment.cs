using Microsoft.EntityFrameworkCore.Migrations;

namespace StoryForce.Server.Migrations
{
    public partial class AddMoreFieldAssignment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "StoryFileAssignment",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "StoryFileAssignment",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "StoryFileAssignment");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "StoryFileAssignment");
        }
    }
}
