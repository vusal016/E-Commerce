namespace Cart.Application.Features.NotifyWishlistItem
{
    public sealed class NotifyWishlistItemCommandHandler(ICartDbContext dbContext, ICatalogPublicApi catalogApi) : IRequestHandler<NotifyWishlistItemCommand, bool>
    {
        public async Task<bool> Handle(NotifyWishlistItemCommand request, CancellationToken cancellationToken)
        {
            if (request.UserId is null || request.UserId == Guid.Empty)
                throw new UnauthorizedAccessException("Only authenticated users can modify wishlists.");

            var wishlist = await dbContext.Wishlists
                .Include(w => w.Items)
                .FirstOrDefaultAsync(w => w.Id == request.WishlistId && (w.UserId == request.UserId), cancellationToken);

            if (wishlist == null)
                throw new KeyNotFoundException("Wishlist not found.");

            var item = wishlist.Items.FirstOrDefault(i => i.Id == request.ItemId);
            if (item == null)
                throw new KeyNotFoundException("Item not found in wishlist.");

            var catalogProducts = await catalogApi.GetBasketProductsAsync(new[] { item.ProductVariantId }, cancellationToken);
            var product = catalogProducts.FirstOrDefault();

            if (product == null)
                throw new KeyNotFoundException("Product variant not found.");

            if (product.StockQuantity > 0)
                throw new InvalidOperationException("Product is already in stock.");

            item.EnableRestockNotification();
            await dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }

}