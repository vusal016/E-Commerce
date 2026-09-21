namespace Catalog.Application.Common.Interfaces
{
    public interface ICatalogDbContext
    {
        DbSet<Product> Products { get; set; }
        DbSet<ProductVariant> ProductVariants { get; set; }
        DbSet<ProductImage> ProductImages { get; set; }
        DbSet<ProductTag> ProductTags { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Brand> Brands { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}