namespace Catalog.Infrastructure.Persistence.Configurations
{
    public sealed class ProductImageConfigurations : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("product_images");  
            builder.HasKey(pi => pi.Id);
        }
    }
}
