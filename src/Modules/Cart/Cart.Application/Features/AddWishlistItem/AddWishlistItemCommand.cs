namespace Cart.Application.Features.AddWishlistItem
{
    public sealed record AddWishlistItemCommand(Guid? UserId, Guid WishlistId, Guid ProductVariantId) : IRequest<Guid>;
}