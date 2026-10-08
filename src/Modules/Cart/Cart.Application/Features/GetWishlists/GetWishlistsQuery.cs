namespace Cart.Application.Features.GetWishlists
{
    public sealed record GetWishlistsQuery(Guid? UserId) : IRequest<IEnumerable<WishlistDto>>;
}