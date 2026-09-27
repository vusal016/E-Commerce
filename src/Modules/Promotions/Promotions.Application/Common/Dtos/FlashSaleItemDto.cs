namespace Promotions.Application.Common.Dtos;

public sealed record FlashSaleItemDto(
    Guid Id,
    Guid ProductVariantId,
    string ProductName,
    string? PrimaryImageUrl,
    decimal OriginalPrice,
    decimal DiscountedPrice,
    decimal DiscountPercentage,
    int StockLimit,
    int SoldCount,
    double SoldPercentage);
