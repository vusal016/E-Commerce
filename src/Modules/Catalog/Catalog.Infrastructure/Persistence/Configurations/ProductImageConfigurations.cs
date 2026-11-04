namespace Catalog.Infrastructure.Persistence.Configurations
{
    internal sealed class ProductImageConfigurations : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("product_images");
            builder.HasKey(pi => pi.Id);
        }
    }
}