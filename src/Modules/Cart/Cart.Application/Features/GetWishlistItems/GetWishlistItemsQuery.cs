namespace Cart.Application.Features.GetWishlistItems
{
    public sealed record GetWishlistItemsQuery(Guid? UserId, Guid WishlistId) : IRequest<IEnumerable<WishlistItemDetailDto>>;
}