namespace Cart.Application.Features.AddWishlistItem
{
    public sealed class AddWishlistItemCommandHandler(ICartDbContext dbContext, ICatalogPublicApi catalogApi) : IRequestHandler<AddWishlistItemCommand, Guid>
    {
        public async Task<Guid> Handle(AddWishlistItemCommand request, CancellationToken cancellationToken)
        {
            if (request.UserId is null || request.UserId == Guid.Empty)
                throw new UnauthorizedAccessException("Only authenticated users can modify wishlists.");

            var wishlist = await dbContext.Wishlists
                .Include(w => w.Items)
                .FirstOrDefaultAsync(w => w.Id == request.WishlistId && (w.UserId == request.UserId), cancellationToken);

            if (wishlist == null)
                throw new KeyNotFoundException("Wishlist not found.");

            if (wishlist.Items.Any(i => i.ProductVariantId == request.ProductVariantId))
                throw new InvalidOperationException("Item already exists in wishlist.");

            var catalogProducts = await catalogApi.GetBasketProductsAsync(new[] { request.ProductVariantId }, cancellationToken);
            var product = catalogProducts.FirstOrDefault();

            if (product == null)
                throw new KeyNotFoundException("Product variant not found.");

            var wishlistItem = new WishlistItem(wishlist.Id, request.ProductVariantId, product.Price);
            wishlist.Items.Add(wishlistItem);

            await dbContext.SaveChangesAsync(cancellationToken);

            return wishlistItem.Id;
        }
    }

}