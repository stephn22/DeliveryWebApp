using Microsoft.EntityFrameworkCore.Migrations;

namespace DeliveryWebApp.WebUI.Migrations
{
    public partial class Currency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "decimal(16,4)",
                precision: 16,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Money",
                oldPrecision: 16,
                oldScale: 3);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                table: "Orders",
                type: "decimal(16,4)",
                precision: 16,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Money",
                oldPrecision: 16,
                oldScale: 3);

            migrationBuilder.AlterColumn<decimal>(
                name: "ProductPrice",
                table: "OrderItems",
                type: "decimal(16,4)",
                precision: 16,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Money",
                oldPrecision: 16,
                oldScale: 3);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                table: "Baskets",
                type: "decimal(16,4)",
                precision: 16,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Money",
                oldPrecision: 16,
                oldScale: 3);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "Money",
                precision: 16,
                scale: 3,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)",
                oldPrecision: 16,
                oldScale: 4);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                table: "Orders",
                type: "Money",
                precision: 16,
                scale: 3,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)",
                oldPrecision: 16,
                oldScale: 4);

            migrationBuilder.AlterColumn<decimal>(
                name: "ProductPrice",
                table: "OrderItems",
                type: "Money",
                precision: 16,
                scale: 3,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)",
                oldPrecision: 16,
                oldScale: 4);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                table: "Baskets",
                type: "Money",
                precision: 16,
                scale: 3,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)",
                oldPrecision: 16,
                oldScale: 4);
        }
    }
}
