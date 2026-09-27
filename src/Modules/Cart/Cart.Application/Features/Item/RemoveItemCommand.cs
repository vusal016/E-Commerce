namespace Cart.Application.Features.Item;

public sealed record RemoveItemCommand(Guid? UserId, string? SessionId, Guid ItemId) : IRequest<CartDto>;



