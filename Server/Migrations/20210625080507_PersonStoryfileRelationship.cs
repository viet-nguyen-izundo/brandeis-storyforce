using Microsoft.EntityFrameworkCore.Migrations;

namespace StoryForce.Server.Migrations
{
    public partial class PersonStoryfileRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_StoryFiles_StoryFileId",
                table: "Persons");

            migrationBuilder.DropIndex(
                name: "IX_Persons_StoryFileId",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "StoryFileId",
                table: "Persons");

            migrationBuilder.CreateTable(
                name: "PersonStoryFile",
                columns: table => new
                {
                    FeaturedPeopleId = table.Column<int>(type: "integer", nullable: false),
                    FeaturedStoryFileId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonStoryFile", x => new { x.FeaturedPeopleId, x.FeaturedStoryFileId });
                    table.ForeignKey(
                        name: "FK_PersonStoryFile_Persons_FeaturedPeopleId",
                        column: x => x.FeaturedPeopleId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonStoryFile_StoryFiles_FeaturedStoryFileId",
                        column: x => x.FeaturedStoryFileId,
                        principalTable: "StoryFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonStoryFile_FeaturedStoryFileId",
                table: "PersonStoryFile",
                column: "FeaturedStoryFileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonStoryFile");

            migrationBuilder.AddColumn<int>(
                name: "StoryFileId",
                table: "Persons",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Persons_StoryFileId",
                table: "Persons",
                column: "StoryFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_StoryFiles_StoryFileId",
                table: "Persons",
                column: "StoryFileId",
                principalTable: "StoryFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
