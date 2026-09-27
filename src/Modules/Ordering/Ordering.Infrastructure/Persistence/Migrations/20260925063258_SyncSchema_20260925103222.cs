using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ordering.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SyncSchema_20260925103222 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_return_requests_OrderItemId",
                schema: "ordering",
                table: "return_requests",
                column: "OrderItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_return_requests_order_items_OrderItemId",
                schema: "ordering",
                table: "return_requests",
                column: "OrderItemId",
                principalSchema: "ordering",
                principalTable: "order_items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_return_requests_order_items_OrderItemId",
                schema: "ordering",
                table: "return_requests");

            migrationBuilder.DropIndex(
                name: "IX_return_requests_OrderItemId",
                schema: "ordering",
                table: "return_requests");
        }
    }
}
