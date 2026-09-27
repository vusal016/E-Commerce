namespace Cart.Application.Common.Dtos
{
    public sealed record CartItemDto(
        Guid Id,
        Guid ProductVariantId,
        int Quantity,
        string ProductName,
        decimal Price,
        string? ImageUrl,
        string? StockWarning
    );
}