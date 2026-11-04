namespace Catalog.Infrastructure.Persistence.Configurations
{
    internal sealed class ProductTagConfigurations : IEntityTypeConfiguration<ProductTag>
    {
        public void Configure(EntityTypeBuilder<ProductTag> builder)
        {
            builder.ToTable("product_tags");
            builder.HasKey(pt => pt.Id);
        }
    }
}