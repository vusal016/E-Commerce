namespace Cart.Application.Common.Dtos;

public sealed record CartSummaryDto(decimal Subtotal, decimal Shipping, decimal Discount, decimal Total);
