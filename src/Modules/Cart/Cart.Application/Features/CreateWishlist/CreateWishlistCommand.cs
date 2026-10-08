namespace Cart.Application.Features.CreateWishlist
{
    public sealed record CreateWishlistCommand(Guid? UserId, string Name) : IRequest<Guid>;
}