namespace Cart.Domain.CartAggregate
{
    public sealed class CartItem : AuditEntity
    {
        private CartItem()
        {
        }

        public CartItem(Guid cartId, Guid productVariantId, int quantity, bool isSavedForLater)
        {
            SetCartId(cartId);
            SetProductVariantId(productVariantId);
            SetQuantity(quantity);
            IsSavedForLater = isSavedForLater;
            AddedAt = DateTime.UtcNow;
        }

        public Guid CartId { get; private set; }
        public Guid ProductVariantId { get; private set; }
        public int Quantity { get; private set; }

        public void UpdateQuantity(int quantity)
        {
            SetQuantity(quantity);
        }

        public void ToggleSaveForLater()
        {
            IsSavedForLater = !IsSavedForLater;
        }
        public bool IsSavedForLater { get; private set; }
        public DateTime AddedAt { get; private set; }

        public Cart Cart { get; private set; } = null!;

        private void SetCartId(Guid cartId)
        {
            if (cartId == Guid.Empty)
                throw new ArgumentException("Cart ID cannot be empty.");
            CartId = cartId;
        }

        private void SetProductVariantId(Guid productVariantId)
        {
            if (productVariantId == Guid.Empty)
                throw new ArgumentException("Product variant ID cannot be empty.");
            ProductVariantId = productVariantId;
        }

        private void SetQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");
            Quantity = quantity;
        }
    }
}



