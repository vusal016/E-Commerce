namespace Cart.Application.Features.CartActions;

public sealed record MergeCartCommand(Guid UserId, string SessionId) : IRequest<CartDto>;



