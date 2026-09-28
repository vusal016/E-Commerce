namespace Ordering.Contracts.Events;

public sealed record OrderItemSnapshot(Guid ProductVariantId, int Quantity);
