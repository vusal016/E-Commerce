namespace Cart.Application.Features.RemoveItem;

public sealed record RemoveItemCommand(Guid? UserId, string? SessionId, Guid ItemId) : IRequest<CartDto>;




