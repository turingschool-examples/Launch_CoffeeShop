using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeShopMVC.Migrations
{
    /// <inheritdoc />
    public partial class UpdateContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_items_order_order_id",
                table: "items");

            migrationBuilder.DropForeignKey(
                name: "fk_order_customers_order_customer_id",
                table: "order");

            migrationBuilder.DropPrimaryKey(
                name: "pk_order",
                table: "order");

            migrationBuilder.RenameTable(
                name: "order",
                newName: "orders");

            migrationBuilder.RenameIndex(
                name: "ix_order_order_customer_id",
                table: "orders",
                newName: "ix_orders_order_customer_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_orders",
                table: "orders",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_items_orders_order_id",
                table: "items",
                column: "order_id",
                principalTable: "orders",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_orders_customers_order_customer_id",
                table: "orders",
                column: "order_customer_id",
                principalTable: "customers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_items_orders_order_id",
                table: "items");

            migrationBuilder.DropForeignKey(
                name: "fk_orders_customers_order_customer_id",
                table: "orders");

            migrationBuilder.DropPrimaryKey(
                name: "pk_orders",
                table: "orders");

            migrationBuilder.RenameTable(
                name: "orders",
                newName: "order");

            migrationBuilder.RenameIndex(
                name: "ix_orders_order_customer_id",
                table: "order",
                newName: "ix_order_order_customer_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_order",
                table: "order",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_items_order_order_id",
                table: "items",
                column: "order_id",
                principalTable: "order",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_order_customers_order_customer_id",
                table: "order",
                column: "order_customer_id",
                principalTable: "customers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
