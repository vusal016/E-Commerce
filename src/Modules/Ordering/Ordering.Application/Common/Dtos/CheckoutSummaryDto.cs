namespace Ordering.Application.Common.Dtos;

public sealed record CheckoutSummaryDto(string? ShippingAddress, string? ShippingMethod, string? PaymentMethod, string? CardLast4, decimal Subtotal, decimal CartDiscount, decimal ShippingPrice, decimal TotalPrice, IReadOnlyList<CheckoutSummaryItemDto> Items);

public sealed record CheckoutSummaryItemDto(Guid ProductVariantId, string ProductName, int Quantity, decimal Price);
