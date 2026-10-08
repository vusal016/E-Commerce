namespace Catalog.Application.Common.Dtos
{
    public sealed record ProductVariantDto(
        Guid Id,
        string Color,
        string Size,
        string Sku,
        decimal Price,
        decimal OriginalPrice,
        int StockQuantity);
}