namespace Cart.Application.Features.ClearCart
{
    public sealed record ClearCartCommand(Guid? UserId, string? SessionId) : IRequest;
}