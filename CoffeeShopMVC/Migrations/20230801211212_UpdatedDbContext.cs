using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeShopMVC.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_item_order_order_orders_id",
                table: "item_order");

            migrationBuilder.DropForeignKey(
                name: "fk_order_customer_customer_id",
                table: "order");

            migrationBuilder.DropPrimaryKey(
                name: "pk_order",
                table: "order");

            migrationBuilder.DropPrimaryKey(
                name: "pk_customer",
                table: "customer");

            migrationBuilder.RenameTable(
                name: "order",
                newName: "orders");

            migrationBuilder.RenameTable(
                name: "customer",
                newName: "customers");

            migrationBuilder.RenameIndex(
                name: "ix_order_customer_id",
                table: "orders",
                newName: "ix_orders_customer_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_orders",
                table: "orders",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_customers",
                table: "customers",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_item_order_orders_orders_id",
                table: "item_order",
                column: "orders_id",
                principalTable: "orders",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_orders_customers_customer_id",
                table: "orders",
                column: "customer_id",
                principalTable: "customers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_item_order_orders_orders_id",
                table: "item_order");

            migrationBuilder.DropForeignKey(
                name: "fk_orders_customers_customer_id",
                table: "orders");

            migrationBuilder.DropPrimaryKey(
                name: "pk_orders",
                table: "orders");

            migrationBuilder.DropPrimaryKey(
                name: "pk_customers",
                table: "customers");

            migrationBuilder.RenameTable(
                name: "orders",
                newName: "order");

            migrationBuilder.RenameTable(
                name: "customers",
                newName: "customer");

            migrationBuilder.RenameIndex(
                name: "ix_orders_customer_id",
                table: "order",
                newName: "ix_order_customer_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_order",
                table: "order",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_customer",
                table: "customer",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_item_order_order_orders_id",
                table: "item_order",
                column: "orders_id",
                principalTable: "order",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_order_customer_customer_id",
                table: "order",
                column: "customer_id",
                principalTable: "customer",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
