using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CoffeeShopMVC.Migrations
{
    /// <inheritdoc />
    public partial class CreateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "customer",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    email_address = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customer", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "items",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    price_in_cents = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_items", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "order",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    customer_id = table.Column<int>(type: "integer", nullable: false),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order", x => x.id);
                    table.ForeignKey(
                        name: "fk_order_customer_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                        name: "fk_item_order_order_orders_id",
                        column: x => x.orders_id,
                        principalTable: "order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_item_order_orders_id",
                table: "item_order",
                column: "orders_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_customer_id",
                table: "order",
                column: "customer_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "item_order");

            migrationBuilder.DropTable(
                name: "items");

            migrationBuilder.DropTable(
                name: "order");

            migrationBuilder.DropTable(
                name: "customer");
        }
    }
}
