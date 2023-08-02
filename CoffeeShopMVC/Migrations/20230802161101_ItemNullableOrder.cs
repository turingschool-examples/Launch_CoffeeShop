using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeShopMVC.Migrations
{
    /// <inheritdoc />
    public partial class ItemNullableOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "item_order");

            migrationBuilder.AddColumn<int>(
                name: "order_id",
                table: "items",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_items_order_id",
                table: "items",
                column: "order_id");

            migrationBuilder.AddForeignKey(
                name: "fk_items_orders_order_id",
                table: "items",
                column: "order_id",
                principalTable: "orders",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_items_orders_order_id",
                table: "items");

            migrationBuilder.DropIndex(
                name: "ix_items_order_id",
                table: "items");

            migrationBuilder.DropColumn(
                name: "order_id",
                table: "items");

            migrationBuilder.CreateTable(
                name: "item_order",
                columns: table => new
                {
                    items_id = table.Column<int>(type: "integer", nullable: false),
                    orders_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_item_order", x => new { x.items_id, x.orders_id });
                    table.ForeignKey(
                        name: "fk_item_order_items_items_id",
                        column: x => x.items_id,
                        principalTable: "items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_item_order_orders_orders_id",
                        column: x => x.orders_id,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_item_order_orders_id",
                table: "item_order",
                column: "orders_id");
        }
    }
}
