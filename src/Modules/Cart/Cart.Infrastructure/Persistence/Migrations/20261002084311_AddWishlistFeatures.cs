using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace Cart.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddWishlistFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ShareToken",
                schema: "cart",
                table: "wishlists",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceAtAdd",
                schema: "cart",
                table: "wishlist_items",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "WantsRestockNotification",
                schema: "cart",
                table: "wishlist_items",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShareToken",
                schema: "cart",
                table: "wishlists");

            migrationBuilder.DropColumn(
                name: "PriceAtAdd",
                schema: "cart",
                table: "wishlist_items");

            migrationBuilder.DropColumn(
                name: "WantsRestockNotification",
                schema: "cart",
                table: "wishlist_items");
        }
    }
}