using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PiikkiTracker.Migrations
{
    /// <inheritdoc />
    public partial class UserJobANDUserProductsChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Remove the existing primary keys
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProducts",
                table: "UserProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserJobs",
                table: "UserJobs");

            // Create temporary columns without identity for data migration
            migrationBuilder.AddColumn<int>(
                name: "TempId_UserProducts",
                table: "UserProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TempId_UserJobs",
                table: "UserJobs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Copy data from the old `Id` column to the new temporary column
            migrationBuilder.Sql("UPDATE UserProducts SET TempId_UserProducts = Id");
            migrationBuilder.Sql("UPDATE UserJobs SET TempId_UserJobs = Id");

            // Drop the old `Id` columns
            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserProducts");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserJobs");

            // Recreate the `Id` columns with identity
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserProducts",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserJobs",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            // Enable IDENTITY_INSERT and copy data into the new identity column
            migrationBuilder.Sql("SET IDENTITY_INSERT UserProducts ON;");
            migrationBuilder.Sql("INSERT INTO UserProducts (Id, UserId, ProductId, Amount, CreatedDate) SELECT TempId_UserProducts, UserId, ProductId, Amount, CreatedDate FROM UserProducts;");
            migrationBuilder.Sql("SET IDENTITY_INSERT UserProducts OFF;");

            migrationBuilder.Sql("SET IDENTITY_INSERT UserJobs ON;");
            migrationBuilder.Sql("INSERT INTO UserJobs (Id, UserId, JobId, Event, Description, IsAccepted, CreatedDate) SELECT TempId_UserJobs, UserId, JobId, Event, Description, IsAccepted, CreatedDate FROM UserJobs;");
            migrationBuilder.Sql("SET IDENTITY_INSERT UserJobs OFF;");

            // Drop the temporary columns
            migrationBuilder.DropColumn(
                name: "TempId_UserProducts",
                table: "UserProducts");

            migrationBuilder.DropColumn(
                name: "TempId_UserJobs",
                table: "UserJobs");

            // Re-add the primary keys
            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProducts",
                table: "UserProducts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserJobs",
                table: "UserJobs",
                column: "Id");

            // Recreate the indices
            migrationBuilder.CreateIndex(
                name: "IX_UserProducts_UserId",
                table: "UserProducts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserJobs_UserId",
                table: "UserJobs",
                column: "UserId");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProducts",
                table: "UserProducts");

            migrationBuilder.DropIndex(
                name: "IX_UserProducts_UserId",
                table: "UserProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserJobs",
                table: "UserJobs");

            migrationBuilder.DropIndex(
                name: "IX_UserJobs_UserId",
                table: "UserJobs");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "UserProducts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "UserJobs",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProducts",
                table: "UserProducts",
                columns: new[] { "UserId", "ProductId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserJobs",
                table: "UserJobs",
                columns: new[] { "UserId", "JobId" });
        }
    }
}
