using Microsoft.EntityFrameworkCore.Migrations;

namespace StoryForce.Server.Migrations
{
    public partial class FixFieldRequestedById : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoryFiles_AspNetUsers_RequestedById",
                table: "StoryFiles");

            migrationBuilder.AlterColumn<int>(
                name: "RequestedById",
                table: "StoryFiles",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_StoryFiles_AspNetUsers_RequestedById",
                table: "StoryFiles",
                column: "RequestedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoryFiles_AspNetUsers_RequestedById",
                table: "StoryFiles");

            migrationBuilder.AlterColumn<int>(
                name: "RequestedById",
                table: "StoryFiles",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StoryFiles_AspNetUsers_RequestedById",
                table: "StoryFiles",
                column: "RequestedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
