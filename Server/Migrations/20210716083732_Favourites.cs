using Microsoft.EntityFrameworkCore.Migrations;

namespace StoryForce.Server.Migrations
{
    public partial class Favourites : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PersonStoryFile1",
                columns: table => new
                {
                    FavouritesPeopleId = table.Column<int>(type: "integer", nullable: false),
                    FavouritesStoryFileId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonStoryFile1", x => new { x.FavouritesPeopleId, x.FavouritesStoryFileId });
                    table.ForeignKey(
                        name: "FK_PersonStoryFile1_AspNetUsers_FavouritesPeopleId",
                        column: x => x.FavouritesPeopleId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonStoryFile1_StoryFiles_FavouritesStoryFileId",
                        column: x => x.FavouritesStoryFileId,
                        principalTable: "StoryFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonStoryFile1_FavouritesStoryFileId",
                table: "PersonStoryFile1",
                column: "FavouritesStoryFileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonStoryFile1");
        }
    }
}
