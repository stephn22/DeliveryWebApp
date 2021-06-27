using Microsoft.EntityFrameworkCore.Migrations;

namespace DeliveryWebApp.WebUI.Migrations
{
    public partial class Customer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Clients_ClientId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Baskets_Clients_ClientId",
                table: "Baskets");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Clients_ClientId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Clients_ClientId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurateurs_Clients_ClientId",
                table: "Restaurateurs");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Clients_ClientId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Riders_Clients_ClientId",
                table: "Riders");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Baskets_ClientId",
                table: "Baskets");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Riders",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Riders_ClientId",
                table: "Riders",
                newName: "IX_Riders_CustomerId");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Review",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Review_ClientId",
                table: "Review",
                newName: "IX_Review_CustomerId");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Restaurateurs",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Restaurateurs_ClientId",
                table: "Restaurateurs",
                newName: "IX_Restaurateurs_CustomerId");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Requests",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Requests_ClientId",
                table: "Requests",
                newName: "IX_Requests_CustomerId");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Orders",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_ClientId",
                table: "Orders",
                newName: "IX_Orders_CustomerId");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Baskets",
                newName: "CustomerId");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Addresses",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_ClientId",
                table: "Addresses",
                newName: "IX_Addresses_CustomerId");

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserFk = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_CustomerId",
                table: "Baskets",
                column: "CustomerId",
                unique: true,
                filter: "[CustomerId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Customers_CustomerId",
                table: "Addresses",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Baskets_Customers_CustomerId",
                table: "Baskets",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                table: "Orders",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Customers_CustomerId",
                table: "Requests",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurateurs_Customers_CustomerId",
                table: "Restaurateurs",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Customers_CustomerId",
                table: "Review",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Riders_Customers_CustomerId",
                table: "Riders",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Customers_CustomerId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Baskets_Customers_CustomerId",
                table: "Baskets");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Customers_CustomerId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurateurs_Customers_CustomerId",
                table: "Restaurateurs");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Customers_CustomerId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Riders_Customers_CustomerId",
                table: "Riders");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Baskets_CustomerId",
                table: "Baskets");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Riders",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Riders_CustomerId",
                table: "Riders",
                newName: "IX_Riders_ClientId");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Review",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Review_CustomerId",
                table: "Review",
                newName: "IX_Review_ClientId");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Restaurateurs",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Restaurateurs_CustomerId",
                table: "Restaurateurs",
                newName: "IX_Restaurateurs_ClientId");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Requests",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Requests_CustomerId",
                table: "Requests",
                newName: "IX_Requests_ClientId");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Orders",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                newName: "IX_Orders_ClientId");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Baskets",
                newName: "ClientId");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Addresses",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_CustomerId",
                table: "Addresses",
                newName: "IX_Addresses_ClientId");

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserFk = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_ClientId",
                table: "Baskets",
                column: "ClientId",
                unique: true,
                filter: "[ClientId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Clients_ClientId",
                table: "Addresses",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Baskets_Clients_ClientId",
                table: "Baskets",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Clients_ClientId",
                table: "Orders",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Clients_ClientId",
                table: "Requests",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurateurs_Clients_ClientId",
                table: "Restaurateurs",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Clients_ClientId",
                table: "Review",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Riders_Clients_ClientId",
                table: "Riders",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
