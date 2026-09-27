namespace Cart.Domain.WishlistAggregate
{
    public sealed class WishlistItem : AuditEntity
    {
        private WishlistItem()
        {
        }

        public WishlistItem(Guid wishlistId, Guid productVariantId)
        {
            SetWishlistId(wishlistId);
            SetProductVariantId(productVariantId);
            AddedAt = DateTime.UtcNow;
        }

        public Guid WishlistId { get; private set; }
        public Guid ProductVariantId { get; private set; }
        public DateTime AddedAt { get; private set; }

        public Wishlist Wishlist { get; private set; } = null!;

        private void SetWishlistId(Guid wishlistId)
        {
            if (wishlistId == Guid.Empty)
                throw new ArgumentException("Wishlist ID cannot be empty.");
            WishlistId = wishlistId;
        }

        private void SetProductVariantId(Guid productVariantId)
        {
            if (productVariantId == Guid.Empty)
                throw new ArgumentException("Product variant ID cannot be empty.");
            ProductVariantId = productVariantId;
        }
    }
}