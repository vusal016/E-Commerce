namespace Catalog.Contracts
{
    public sealed record ProductSummaryDto(
        Guid ProductId,
        Guid VariantId,
        string ProductName,
        string? PrimaryImageUrl,
        decimal Price,
        int StockQuantity
    );
}