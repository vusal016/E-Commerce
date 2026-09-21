namespace Catalog.Domain.ProductAggregate
{
    public sealed class ProductVariant : BaseEntity
    {
        private ProductVariant()
        {
        }

        public ProductVariant(Guid productId, string color, string size, string sku, decimal price, decimal originalPrice, int stockQuantity, bool isActive)
        {
            SetProductId(productId);
            SetColor(color);
            SetSize(size);
            SetSku(sku);
            SetPrice(price);
            SetOriginalPrice(originalPrice);
            SetStockQuantity(stockQuantity);
            IsActive = isActive;
        }
        public Guid ProductId { get; private set; }
        public Product Product { get;private set; }
        public string Color { get;private set; }
        public string Size { get; private set; }
        public string Sku { get; private set; }
        public decimal Price { get;private set; }
        public decimal OriginalPrice { get;private set; }
        public int StockQuantity { get; private set; }
        public bool IsActive { get; private set; }

        private void SetColor(string color)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(color, "Product variant color cannot be empty.");
            Color = color;
        }

        private void SetSize(string size)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(size, "Product variant size cannot be empty.");
            Size = size;
        }

        private void SetSku(string sku)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(sku, "Product variant SKU cannot be empty.");
            Sku = sku;
        }
        private void SetPrice(decimal price)
        {
            if (price < 0)
                throw new ArgumentException("Product variant price cannot be negative.");
            Price = price;
        }
        private void SetOriginalPrice(decimal originalPrice)
        {
            if (originalPrice < 0)
                throw new ArgumentException("Product variant original price cannot be negative.");
            OriginalPrice = originalPrice;
        }
        private void SetStockQuantity(int stockQuantity)
        {
            if (stockQuantity < 0)
                throw new ArgumentException("Product variant stock quantity cannot be negative.");
            StockQuantity = stockQuantity;
        }
        private void SetProductId(Guid productId)
        {
            if (productId == Guid.Empty)
                throw new ArgumentException("Product ID cannot be empty.");
            ProductId = productId;
        }
    }
}
