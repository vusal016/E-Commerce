namespace Cart.Application.Features.Item;

public sealed record ToggleSaveForLaterCommand(Guid? UserId, string? SessionId, Guid ItemId) : IRequest<CartDto>;



