using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class addRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Availabilities_Products_ProductId",
                table: "Availabilities");

            migrationBuilder.DropIndex(
                name: "IX_Availabilities_ProductId",
                table: "Availabilities");

            migrationBuilder.AddColumn<int>(
                name: "VariantId",
                table: "CartItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VariantId",
                table: "Availabilities",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VariantId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "VariantId",
                table: "Availabilities");

            migrationBuilder.CreateIndex(
                name: "IX_Availabilities_ProductId",
                table: "Availabilities",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Availabilities_Products_ProductId",
                table: "Availabilities",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
