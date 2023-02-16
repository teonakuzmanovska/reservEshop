using Microsoft.EntityFrameworkCore.Migrations;

namespace EShop.Repository.Migrations
{
    public partial class AddedSizePropertyForProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductRating",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "ProductSize",
                table: "ProductsInShoppingCarts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Size",
                table: "ProductsInOrders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductSize",
                table: "Products",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductSize",
                table: "ProductsInShoppingCarts");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "ProductsInOrders");

            migrationBuilder.DropColumn(
                name: "ProductSize",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "ProductRating",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
