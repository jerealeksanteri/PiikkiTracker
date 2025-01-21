using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PiikkiTracker.Migrations
{
    /// <inheritdoc />
    public partial class ChangesBalanceSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserJobs_Transaction_TransactionId",
                table: "UserJobs");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProducts_Tab_TabId",
                table: "UserProducts");

            migrationBuilder.DropTable(
                name: "Tab");

            migrationBuilder.DropIndex(
                name: "IX_UserProducts_TabId",
                table: "UserProducts");

            migrationBuilder.DropIndex(
                name: "IX_UserJobs_TransactionId",
                table: "UserJobs");

            migrationBuilder.DropColumn(
                name: "TabAddedDate",
                table: "UserProducts");

            migrationBuilder.DropColumn(
                name: "TabId",
                table: "UserProducts");

            migrationBuilder.DropColumn(
                name: "TransactionAddedDate",
                table: "UserJobs");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "UserJobs");

            migrationBuilder.DropColumn(
                name: "JobTotal",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "TabsTotal",
                table: "Transaction");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "Transaction",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "TransactionType",
                table: "Transaction",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "TransactionType",
                table: "Transaction");

            migrationBuilder.AddColumn<DateTime>(
                name: "TabAddedDate",
                table: "UserProducts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TabId",
                table: "UserProducts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TransactionAddedDate",
                table: "UserJobs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransactionId",
                table: "UserJobs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "JobTotal",
                table: "Transaction",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TabsTotal",
                table: "Transaction",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Tab",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsTabOpen = table.Column<bool>(type: "bit", nullable: false),
                    TabClosed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TabOpened = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TabTotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tab", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tab_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tab_Transaction_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transaction",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProducts_TabId",
                table: "UserProducts",
                column: "TabId");

            migrationBuilder.CreateIndex(
                name: "IX_UserJobs_TransactionId",
                table: "UserJobs",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Tab_TransactionId",
                table: "Tab",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Tab_UserId",
                table: "Tab",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserJobs_Transaction_TransactionId",
                table: "UserJobs",
                column: "TransactionId",
                principalTable: "Transaction",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProducts_Tab_TabId",
                table: "UserProducts",
                column: "TabId",
                principalTable: "Tab",
                principalColumn: "Id");
        }
    }
}
