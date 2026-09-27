namespace Cart.Application.Features.Item;

public sealed record UpdateItemQuantityCommand(Guid? UserId, string? SessionId, Guid ItemId, int Quantity) : IRequest<CartDto>;



