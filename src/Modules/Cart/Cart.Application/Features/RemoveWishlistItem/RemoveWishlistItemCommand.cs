namespace Cart.Application.Features.RemoveWishlistItem
{
    public sealed record RemoveWishlistItemCommand(Guid? UserId, Guid WishlistId, Guid ItemId) : IRequest<bool>;

}