namespace Cart.Application.Features.MergeCart
{
    public sealed record MergeCartCommand(Guid UserId, string SessionId) : IRequest<CartDto>;
}