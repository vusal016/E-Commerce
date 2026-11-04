namespace Cart.Application.Features.ClearCart
{
    internal sealed record ClearCartCommand(Guid? UserId, string? SessionId) : IRequest;
}