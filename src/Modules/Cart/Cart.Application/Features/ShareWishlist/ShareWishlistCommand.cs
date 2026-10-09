namespace Cart.Application.Features.ShareWishlist
{
    public sealed record ShareWishlistCommand(Guid? UserId, Guid WishlistId, string BaseUrl) : IRequest<string>;
}