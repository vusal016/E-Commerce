namespace Cart.Domain.WishlistAggregate
{
    public sealed class WishlistItem : AuditEntity
    {
        private WishlistItem()
        {
        }

        public WishlistItem(Guid wishlistId, Guid productVariantId, decimal priceAtAdd)
        {
            SetWishlistId(wishlistId);
            SetProductVariantId(productVariantId);
            AddedAt = DateTime.UtcNow;
            PriceAtAdd = priceAtAdd;
        }

        public Guid WishlistId { get; private set; }
        public Guid ProductVariantId { get; private set; }
        public DateTime AddedAt { get; private set; }
        public decimal PriceAtAdd { get; private set; }
        public bool WantsRestockNotification { get; private set; }
        public void EnableRestockNotification() { WantsRestockNotification = true; }

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