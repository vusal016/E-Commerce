namespace Ordering.Application.Common.Dtos;
public sealed record OrderItemDto(Guid Id, Guid ProductVariantId, string ProductName, decimal Price, int Quantity);
