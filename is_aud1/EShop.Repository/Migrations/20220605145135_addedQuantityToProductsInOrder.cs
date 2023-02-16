using Microsoft.EntityFrameworkCore.Migrations;

namespace EShop.Repository.Migrations
{
    public partial class addedQuantityToProductsInOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "ProductsInOrders",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "ProductsInOrders");
        }
    }
}
