using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PiikkiTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddUserProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserJob_AspNetUsers_UserId",
                table: "UserJob");

            migrationBuilder.DropForeignKey(
                name: "FK_UserJob_Jobs_JobId",
                table: "UserJob");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserJob",
                table: "UserJob");

            migrationBuilder.RenameTable(
                name: "UserJob",
                newName: "UserJobs");

            migrationBuilder.RenameIndex(
                name: "IX_UserJob_JobId",
                table: "UserJobs",
                newName: "IX_UserJobs_JobId");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TelegramNickname",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserJobs",
                table: "UserJobs",
                columns: new[] { "UserId", "JobId" });

            migrationBuilder.CreateTable(
                name: "UserProducts",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProducts", x => new { x.UserId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_UserProducts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProducts_ProductId",
                table: "UserProducts",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserJobs_AspNetUsers_UserId",
                table: "UserJobs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserJobs_Jobs_JobId",
                table: "UserJobs",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserJobs_AspNetUsers_UserId",
                table: "UserJobs");

            migrationBuilder.DropForeignKey(
                name: "FK_UserJobs_Jobs_JobId",
                table: "UserJobs");

            migrationBuilder.DropTable(
                name: "UserProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserJobs",
                table: "UserJobs");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TelegramNickname",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "UserJobs",
                newName: "UserJob");

            migrationBuilder.RenameIndex(
                name: "IX_UserJobs_JobId",
                table: "UserJob",
                newName: "IX_UserJob_JobId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserJob",
                table: "UserJob",
                columns: new[] { "UserId", "JobId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserJob_AspNetUsers_UserId",
                table: "UserJob",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserJob_Jobs_JobId",
                table: "UserJob",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
