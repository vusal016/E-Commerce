namespace Promotions.Contracts;

public sealed record ActiveDiscountDto(
    Guid ProductVariantId,
    decimal DiscountPercentage);
