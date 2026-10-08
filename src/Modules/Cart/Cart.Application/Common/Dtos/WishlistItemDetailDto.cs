namespace Cart.Application.Common.Dtos
{
    public sealed record WishlistItemDetailDto(
        Guid Id,
        Guid ProductVariantId,
        string ProductName,
        string? Color,
        string? Size,
        string? PrimaryImageUrl,
        decimal CurrentPrice,
        decimal PriceAtAdd,
        IReadOnlyList<string> DynamicStatuses
    );

}