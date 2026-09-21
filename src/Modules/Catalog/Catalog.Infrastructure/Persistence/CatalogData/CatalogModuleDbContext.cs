namespace Catalog.Infrastructure.Persistence.CatalogData
{
    public sealed class CatalogModuleDbContext(DbContextOptions<CatalogModuleDbContext> options) : DbContext(options), ICatalogDbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("Catalog");
            builder.ApplyConfigurationsFromAssembly(typeof(CatalogModuleDbContext).Assembly);
        }
    }
}