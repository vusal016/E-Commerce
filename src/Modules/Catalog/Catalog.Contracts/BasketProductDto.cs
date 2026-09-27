namespace Catalog.Contracts
{
    public sealed record BasketProductDto
    (
        Guid VariantId,
        string ProductName,
        string Color,
        string Size,
        string? PrimaryImageUrl,
        decimal Price,
        decimal OriginalPrice,
        int StockQuantity
    );
}
