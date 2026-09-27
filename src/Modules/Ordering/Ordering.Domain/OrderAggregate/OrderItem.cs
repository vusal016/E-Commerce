namespace Ordering.Domain.OrderAggregate
{
    public sealed class OrderItem : AuditEntity
    {
        private OrderItem()
        {
        }

        public OrderItem(Guid orderId, Guid productVariantId, string productNameSnapshot, string? colorSnapshot, string? sizeSnapshot, decimal priceSnapshot, int quantity)
        {
            SetOrderId(orderId);
            SetProductVariantId(productVariantId);
            SetSnapshots(productNameSnapshot, colorSnapshot, sizeSnapshot, priceSnapshot);
            SetQuantity(quantity);
        }

        public Guid OrderId { get; private set; }
        public Guid ProductVariantId { get; private set; }
        public string ProductNameSnapshot { get; private set; }
        public string? ColorSnapshot { get; private set; }
        public string? SizeSnapshot { get; private set; }
        public decimal PriceSnapshot { get; private set; }
        public int Quantity { get; private set; }

        public Order Order { get; private set; } = null!;

        private void SetOrderId(Guid orderId)
        {
            if (orderId == Guid.Empty)
                throw new ArgumentException("Order ID cannot be empty.");
            OrderId = orderId;
        }
        
        private void SetProductVariantId(Guid productVariantId)
        {
            if (productVariantId == Guid.Empty)
                throw new ArgumentException("Product variant ID cannot be empty.");
            ProductVariantId = productVariantId;
        }

        private void SetSnapshots(string productNameSnapshot, string? colorSnapshot, string? sizeSnapshot, decimal priceSnapshot)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(productNameSnapshot, "Product name snapshot cannot be empty.");
            if (priceSnapshot < 0)
                throw new ArgumentException("Price snapshot cannot be negative.");
            
            ProductNameSnapshot = productNameSnapshot;
            ColorSnapshot = colorSnapshot;
            SizeSnapshot = sizeSnapshot;
            PriceSnapshot = priceSnapshot;
        }

        private void SetQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");
            Quantity = quantity;
        }
    }
}
