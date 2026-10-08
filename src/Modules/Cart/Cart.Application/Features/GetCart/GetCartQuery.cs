namespace Cart.Application.Features.Queries
{
    public sealed record GetCartQuery(Guid? UserId, string? SessionId) : IRequest<CartDto>;
}