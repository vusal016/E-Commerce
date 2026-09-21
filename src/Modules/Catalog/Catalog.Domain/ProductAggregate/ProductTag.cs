namespace Catalog.Domain.ProductAggregate
{
    public sealed class ProductTag : BaseEntity
    {
        private ProductTag()
        {
        }

        public ProductTag(Guid productId, TagType tagType, string value)
        {
            SetProductId(productId);
            SetTagType(tagType);
            SetValue(value);
        }
        public Guid ProductId { get; private set; }
        public Product Product { get; private set; }
        public TagType TagType { get; private set; }
        public string Value { get; private set; }

        private void SetProductId(Guid productId)
        {
            if (productId == Guid.Empty)
                throw new ArgumentException("Product ID cannot be empty.");
            ProductId = productId;
        }
        private void SetValue(string value)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value, "Product tag value cannot be empty.");
            Value = value;
        }
        private void SetTagType(TagType tagType)
        {
            if (!Enum.IsDefined(typeof(TagType), tagType))
                throw new ArgumentException("Invalid tag type.");
            TagType = tagType;
        }
    }
}