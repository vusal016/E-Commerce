namespace Cart.Application.Features.ShareWishlist
{
    public sealed class ShareWishlistCommandHandler(ICartDbContext dbContext) : IRequestHandler<ShareWishlistCommand, string>
    {
        public async Task<string> Handle(ShareWishlistCommand request, CancellationToken cancellationToken)
        {
            if (request.UserId is null || request.UserId == Guid.Empty)
                throw new UnauthorizedAccessException("Only authenticated users can share wishlists.");

            var wishlist = await dbContext.Wishlists
                .FirstOrDefaultAsync(w => w.Id == request.WishlistId && w.UserId == request.UserId, cancellationToken);

            if (wishlist == null)
                throw new KeyNotFoundException("Wishlist not found.");

            wishlist.GenerateShareToken();
            await dbContext.SaveChangesAsync(cancellationToken);

            return $"{request.BaseUrl.TrimEnd('/')}/shared-wishlists/{wishlist.ShareToken}";
        }
    }
}