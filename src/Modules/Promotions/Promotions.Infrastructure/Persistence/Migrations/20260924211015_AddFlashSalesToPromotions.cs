
#nullable disable

namespace Promotions.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFlashSalesToPromotions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "flash_sales",
                schema: "promotions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    StartsAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndsAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_flash_sales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "flash_sale_items",
                schema: "promotions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FlashSaleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductVariantId = table.Column<Guid>(type: "uuid", nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "numeric", nullable: false),
                    SoldCount = table.Column<int>(type: "integer", nullable: false),
                    StockLimit = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_flash_sale_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_flash_sale_items_flash_sales_FlashSaleId",
                        column: x => x.FlashSaleId,
                        principalSchema: "promotions",
                        principalTable: "flash_sales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_flash_sale_items_FlashSaleId",
                schema: "promotions",
                table: "flash_sale_items",
                column: "FlashSaleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "flash_sale_items",
                schema: "promotions");

            migrationBuilder.DropTable(
                name: "flash_sales",
                schema: "promotions");
        }
    }
}
