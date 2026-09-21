namespace Catalog.Infrastructure.Persistence.Configurations
{
    public sealed class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
           builder.ToTable("products");
            builder.HasKey(p => p.Id);

            builder.HasMany(p => p.ProductVariants)
                .WithOne(p => p.Product)
                .HasForeignKey(pv => pv.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.ProductImages)
                .WithOne(p => p.Product)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.ProductTags)
                .WithOne(p => p.Product)
                .HasForeignKey(pt => pt.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Brand>()
                .WithMany()
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Category>()
                .WithMany()
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}