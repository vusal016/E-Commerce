namespace Cart.Application.Features.NotifyWishlistItem
{
    public sealed record NotifyWishlistItemCommand(Guid? UserId, Guid WishlistId, Guid ItemId) : IRequest<bool>;

}