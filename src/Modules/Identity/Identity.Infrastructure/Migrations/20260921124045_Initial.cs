using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "payment_methods",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CardType = table.Column<int>(type: "integer", nullable: false),
                    LastFourDigits = table.Column<string>(type: "text", nullable: false),
                    ExpiryMonth = table.Column<int>(type: "integer", nullable: false),
                    ExpiryYear = table.Column<int>(type: "integer", nullable: false),
                    CardHolderName = table.Column<string>(type: "text", nullable: false),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_methods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_payment_methods_users_UserId",
                        column: x => x.UserId,
                        principalSchema: "identity",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_refresh_tokens_UserId",
                schema: "identity",
                table: "refresh_tokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_payment_methods_UserId",
                schema: "identity",
                table: "payment_methods",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_refresh_tokens_users_UserId",
                schema: "identity",
                table: "refresh_tokens",
                column: "UserId",
                principalSchema: "identity",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_refresh_tokens_users_UserId",
                schema: "identity",
                table: "refresh_tokens");

            migrationBuilder.DropTable(
                name: "payment_methods",
                schema: "identity");

            migrationBuilder.DropIndex(
                name: "IX_refresh_tokens_UserId",
                schema: "identity",
                table: "refresh_tokens");
        }
    }
}
