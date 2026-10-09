namespace Cart.Application.Features.RemoveWishlistItem
{
    public sealed class RemoveWishlistItemCommandHandler(ICartDbContext dbContext) : IRequestHandler<RemoveWishlistItemCommand, bool>
    {
        public async Task<bool> Handle(RemoveWishlistItemCommand request, CancellationToken cancellationToken)
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

            wishlist.Items.Remove(item);
            await dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}