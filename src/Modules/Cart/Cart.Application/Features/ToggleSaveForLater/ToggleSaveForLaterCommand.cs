namespace Cart.Application.Features.ToggleSaveForLater
{
    public sealed record ToggleSaveForLaterCommand(Guid? UserId, string? SessionId, Guid ItemId) : IRequest<CartDto>;
}