namespace Catalog.Domain.ProductAggregate
{
    public sealed class Product : AuditEntity
    {
        private Product()
        {

        }
        public Product(string name, string description, decimal basePrice, bool isActive, Guid brandId, Guid categoryId)
        {
            SetName(name);
            SetDescription(description);
            SetBasePrice(basePrice);
            IsActive = isActive;
            SetBrandId(brandId);
            SetCategoryId(categoryId);
        }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal BasePrice { get; private set; }
        public bool IsActive { get; private set; }
        public Guid BrandId { get; private set; }
        public Guid CategoryId { get; private set; }
        public ICollection<ProductVariant> ProductVariants { get; private set; } = [];
        public ICollection<ProductImage> ProductImages { get; private set; } = [];
        public ICollection<ProductTag> ProductTags { get; private set; } = [];

        private void SetName(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name, "Product name cannot be empty.");
            Name = name;
        }
        private void SetDescription(string description)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(description, "Product description cannot be empty.");
            Description = description;
        }
        private void SetBasePrice(decimal basePrice)
        {
            if (basePrice < 0)
                throw new ArgumentException("Product base price cannot be negative.");
            BasePrice = basePrice;
        }

        private void SetBrandId(Guid brandId)
        {
            if (brandId == Guid.Empty)
                throw new ArgumentException("Brand ID cannot be empty.");
            BrandId = brandId;
        }
        private void SetCategoryId(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
                throw new ArgumentException("Category ID cannot be empty.");
            CategoryId = categoryId;
        }
    }
}