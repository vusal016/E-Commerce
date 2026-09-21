namespace Catalog.Infrastructure.Persistence.Configurations
{
    public sealed class ProductVariantConfigurations : IEntityTypeConfiguration<ProductVariant>
    {
        public void Configure(EntityTypeBuilder<ProductVariant> builder)
        {
            builder.ToTable("product_variants");
            builder.HasKey(pv => pv.Id);

            builder.HasIndex(pv => pv.Sku).IsUnique();
        }
    }
}
