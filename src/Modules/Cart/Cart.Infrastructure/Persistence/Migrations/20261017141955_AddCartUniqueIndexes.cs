using Microsoft.EntityFrameworkCore.Migrations;
#nullable disable
namespace Cart.Infrastructure.Persistence.Migrations
{
    public partial class AddCartUniqueIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_wishlist_items_WishlistId",
                schema: "cart",
                table: "wishlist_items");
            migrationBuilder.DropIndex(
                name: "IX_cart_items_CartId",
                schema: "cart",
                table: "cart_items");
            migrationBuilder.Sql("DELETE FROM cart.wishlist_items a USING cart.wishlist_items b WHERE a.ctid < b.ctid AND a.\"WishlistId\" = b.\"WishlistId\" AND a.\"ProductVariantId\" = b.\"ProductVariantId\";");
            migrationBuilder.Sql("DELETE FROM cart.cart_items a USING cart.cart_items b WHERE a.ctid < b.ctid AND a.\"CartId\" = b.\"CartId\" AND a.\"ProductVariantId\" = b.\"ProductVariantId\";");
            migrationBuilder.Sql("DELETE FROM cart.carts a USING cart.carts b WHERE a.ctid < b.ctid AND a.\"UserId\" IS NULL AND b.\"UserId\" IS NULL AND a.\"SessionId\" = b.\"SessionId\";");
            migrationBuilder.Sql("DELETE FROM cart.carts a USING cart.carts b WHERE a.ctid < b.ctid AND a.\"UserId\" IS NOT NULL AND a.\"UserId\" = b.\"UserId\";");
            migrationBuilder.CreateIndex(
                name: "IX_wishlist_items_WishlistId_ProductVariantId",
                schema: "cart",
                table: "wishlist_items",
                columns: new[] { "WishlistId", "ProductVariantId" },
                unique: true);
            migrationBuilder.CreateIndex(
                name: "IX_carts_SessionId",
                schema: "cart",
                table: "carts",
                column: "SessionId",
                unique: true,
                filter: "\"UserId\" IS NULL");
            migrationBuilder.CreateIndex(
                name: "IX_carts_UserId",
                schema: "cart",
                table: "carts",
                column: "UserId",
                unique: true,
                filter: "\"UserId\" IS NOT NULL");
            migrationBuilder.CreateIndex(
                name: "IX_cart_items_CartId_ProductVariantId",
                schema: "cart",
                table: "cart_items",
                columns: new[] { "CartId", "ProductVariantId" },
                unique: true);
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_wishlist_items_WishlistId_ProductVariantId",
                schema: "cart",
                table: "wishlist_items");
            migrationBuilder.DropIndex(
                name: "IX_carts_SessionId",
                schema: "cart",
                table: "carts");
            migrationBuilder.DropIndex(
                name: "IX_carts_UserId",
                schema: "cart",
                table: "carts");
            migrationBuilder.DropIndex(
                name: "IX_cart_items_CartId_ProductVariantId",
                schema: "cart",
                table: "cart_items");
            migrationBuilder.CreateIndex(
                name: "IX_wishlist_items_WishlistId",
                schema: "cart",
                table: "wishlist_items",
                column: "WishlistId");
            migrationBuilder.CreateIndex(
                name: "IX_cart_items_CartId",
                schema: "cart",
                table: "cart_items",
                column: "CartId");
        }
    }
}
