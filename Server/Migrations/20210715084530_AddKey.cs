using Microsoft.EntityFrameworkCore.Migrations;

namespace StoryForce.Server.Migrations
{
    public partial class AddKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoryFiles_AspNetUsers_UpdatedById",
                table: "StoryFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_StoryFiles_Submissions_ApprovedSubmissionId",
                table: "StoryFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_StoryFiles_Submissions_RejectedSubmissionId",
                table: "StoryFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_AspNetUsers_ApprovedById",
                table: "Submissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_AspNetUsers_ReviewedById",
                table: "Submissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_AspNetUsers_SubmittedById",
                table: "Submissions");

            migrationBuilder.AddForeignKey(
                name: "FK_StoryFiles_AspNetUsers_UpdatedById",
                table: "StoryFiles",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StoryFiles_Submissions_ApprovedSubmissionId",
                table: "StoryFiles",
                column: "ApprovedSubmissionId",
                principalTable: "Submissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StoryFiles_Submissions_RejectedSubmissionId",
                table: "StoryFiles",
                column: "RejectedSubmissionId",
                principalTable: "Submissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_AspNetUsers_ApprovedById",
                table: "Submissions",
                column: "ApprovedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_AspNetUsers_ReviewedById",
                table: "Submissions",
                column: "ReviewedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_AspNetUsers_SubmittedById",
                table: "Submissions",
                column: "SubmittedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoryFiles_AspNetUsers_UpdatedById",
                table: "StoryFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_StoryFiles_Submissions_ApprovedSubmissionId",
                table: "StoryFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_StoryFiles_Submissions_RejectedSubmissionId",
                table: "StoryFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_AspNetUsers_ApprovedById",
                table: "Submissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_AspNetUsers_ReviewedById",
                table: "Submissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_AspNetUsers_SubmittedById",
                table: "Submissions");

            migrationBuilder.AddForeignKey(
                name: "FK_StoryFiles_AspNetUsers_UpdatedById",
                table: "StoryFiles",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoryFiles_Submissions_ApprovedSubmissionId",
                table: "StoryFiles",
                column: "ApprovedSubmissionId",
                principalTable: "Submissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoryFiles_Submissions_RejectedSubmissionId",
                table: "StoryFiles",
                column: "RejectedSubmissionId",
                principalTable: "Submissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_AspNetUsers_ApprovedById",
                table: "Submissions",
                column: "ApprovedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_AspNetUsers_ReviewedById",
                table: "Submissions",
                column: "ReviewedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_AspNetUsers_SubmittedById",
                table: "Submissions",
                column: "SubmittedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
