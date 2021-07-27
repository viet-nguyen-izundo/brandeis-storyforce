using Microsoft.EntityFrameworkCore.Migrations;

namespace StoryForce.Server.Migrations
{
    public partial class UserFavourites : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonStoryFile1_AspNetUsers_FavouritesPeopleId",
                table: "PersonStoryFile1");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonStoryFile1_StoryFiles_FavouritesStoryFileId",
                table: "PersonStoryFile1");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonStoryFile1",
                table: "PersonStoryFile1");

            migrationBuilder.RenameTable(
                name: "PersonStoryFile1",
                newName: "Favorites");

            migrationBuilder.RenameIndex(
                name: "IX_PersonStoryFile1_FavouritesStoryFileId",
                table: "Favorites",
                newName: "IX_Favorites_FavouritesStoryFileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Favorites",
                table: "Favorites",
                columns: new[] { "FavouritesPeopleId", "FavouritesStoryFileId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_AspNetUsers_FavouritesPeopleId",
                table: "Favorites",
                column: "FavouritesPeopleId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_StoryFiles_FavouritesStoryFileId",
                table: "Favorites",
                column: "FavouritesStoryFileId",
                principalTable: "StoryFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_AspNetUsers_FavouritesPeopleId",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_StoryFiles_FavouritesStoryFileId",
                table: "Favorites");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Favorites",
                table: "Favorites");

            migrationBuilder.RenameTable(
                name: "Favorites",
                newName: "PersonStoryFile1");

            migrationBuilder.RenameIndex(
                name: "IX_Favorites_FavouritesStoryFileId",
                table: "PersonStoryFile1",
                newName: "IX_PersonStoryFile1_FavouritesStoryFileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonStoryFile1",
                table: "PersonStoryFile1",
                columns: new[] { "FavouritesPeopleId", "FavouritesStoryFileId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PersonStoryFile1_AspNetUsers_FavouritesPeopleId",
                table: "PersonStoryFile1",
                column: "FavouritesPeopleId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonStoryFile1_StoryFiles_FavouritesStoryFileId",
                table: "PersonStoryFile1",
                column: "FavouritesStoryFileId",
                principalTable: "StoryFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
