namespace Cart.Application.Features.Item;

public sealed record AddItemCommand(Guid? UserId, string? SessionId, Guid ProductVariantId, int Quantity) : IRequest<CartDto>;



