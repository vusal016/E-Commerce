using Microsoft.EntityFrameworkCore.Migrations;
#nullable disable
namespace Ordering.Infrastructure.Persistence.Migrations
{
    public partial class AddOrderingUniqueIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_checkout_sessions_SessionId",
                schema: "ordering",
                table: "checkout_sessions");
            migrationBuilder.DropIndex(
                name: "IX_checkout_sessions_UserId",
                schema: "ordering",
                table: "checkout_sessions");
            migrationBuilder.Sql("DELETE FROM ordering.checkout_sessions a USING ordering.checkout_sessions b WHERE a.ctid < b.ctid AND a.\"IsCompleted\" = false AND b.\"IsCompleted\" = false AND a.\"UserId\" IS NULL AND b.\"UserId\" IS NULL AND a.\"SessionId\" = b.\"SessionId\";");
            migrationBuilder.Sql("DELETE FROM ordering.checkout_sessions a USING ordering.checkout_sessions b WHERE a.ctid < b.ctid AND a.\"IsCompleted\" = false AND b.\"IsCompleted\" = false AND a.\"UserId\" IS NOT NULL AND a.\"UserId\" = b.\"UserId\";");
            migrationBuilder.CreateIndex(
                name: "IX_checkout_sessions_SessionId",
                schema: "ordering",
                table: "checkout_sessions",
                column: "SessionId",
                unique: true,
                filter: "\"UserId\" IS NULL AND \"IsCompleted\" = false");
            migrationBuilder.CreateIndex(
                name: "IX_checkout_sessions_UserId",
                schema: "ordering",
                table: "checkout_sessions",
                column: "UserId",
                unique: true,
                filter: "\"UserId\" IS NOT NULL AND \"IsCompleted\" = false");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_checkout_sessions_SessionId",
                schema: "ordering",
                table: "checkout_sessions");
            migrationBuilder.DropIndex(
                name: "IX_checkout_sessions_UserId",
                schema: "ordering",
                table: "checkout_sessions");
            migrationBuilder.CreateIndex(
                name: "IX_checkout_sessions_SessionId",
                schema: "ordering",
                table: "checkout_sessions",
                column: "SessionId");
            migrationBuilder.CreateIndex(
                name: "IX_checkout_sessions_UserId",
                schema: "ordering",
                table: "checkout_sessions",
                column: "UserId");
        }
    }
}
