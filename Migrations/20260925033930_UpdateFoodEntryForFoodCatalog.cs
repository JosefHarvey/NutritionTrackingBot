using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NutritionTrackingBot.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFoodEntryForFoodCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodEntries_Users_userId",
                table: "FoodEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFoodCatalogs_Users_UserId",
                table: "UserFoodCatalogs");

            migrationBuilder.AlterColumn<int>(
                name: "userId",
                table: "FoodEntries",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FoodCatalogId",
                table: "FoodEntries",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Quantity",
                table: "FoodEntries",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Unit",
                table: "FoodEntries",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FoodEntries_FoodCatalogId",
                table: "FoodEntries",
                column: "FoodCatalogId");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodEntries_FoodCatalogs_FoodCatalogId",
                table: "FoodEntries",
                column: "FoodCatalogId",
                principalTable: "FoodCatalogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FoodEntries_Users_userId",
                table: "FoodEntries",
                column: "userId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFoodCatalogs_Users_UserId",
                table: "UserFoodCatalogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodEntries_FoodCatalogs_FoodCatalogId",
                table: "FoodEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_FoodEntries_Users_userId",
                table: "FoodEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFoodCatalogs_Users_UserId",
                table: "UserFoodCatalogs");

            migrationBuilder.DropIndex(
                name: "IX_FoodEntries_FoodCatalogId",
                table: "FoodEntries");

            migrationBuilder.DropColumn(
                name: "FoodCatalogId",
                table: "FoodEntries");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "FoodEntries");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "FoodEntries");

            migrationBuilder.AlterColumn<int>(
                name: "userId",
                table: "FoodEntries",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodEntries_Users_userId",
                table: "FoodEntries",
                column: "userId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFoodCatalogs_Users_UserId",
                table: "UserFoodCatalogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
