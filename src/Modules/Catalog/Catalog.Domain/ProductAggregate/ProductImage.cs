namespace Catalog.Domain.ProductAggregate
{
    public sealed class ProductImage : BaseEntity
    {
        private ProductImage()
        {
        }

        public ProductImage(Guid productId, string imageUrl, int displayOrder, bool isPrimary)
        {
            SetProductId(productId);
            SetImageUrl(imageUrl);
            SetDisplayOrder(displayOrder);
            IsPrimary = isPrimary;
        }

        public Guid ProductId { get; private set; }
        public Product Product { get; private set; }
        public string ImageUrl { get; private set; }
        public int DisplayOrder { get; private set; }
        public bool IsPrimary { get; private set; }

        private void SetImageUrl(string imageUrl)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(imageUrl, "Product image URL cannot be empty.");
            ImageUrl = imageUrl;
        }
        private void SetDisplayOrder(int displayOrder)
        {
            if (displayOrder < 0)
                throw new ArgumentException("Product image display order cannot be negative.");
            DisplayOrder = displayOrder;
        }
        private void SetProductId(Guid productId)
        {
            if (productId == Guid.Empty)
                throw new ArgumentException("Product ID cannot be empty.");
            ProductId = productId;
        }

    }
}
