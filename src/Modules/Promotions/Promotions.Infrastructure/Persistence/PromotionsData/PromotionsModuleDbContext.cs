namespace Promotions.Infrastructure.Persistence.PromotionsData
{
    public sealed class PromotionsModuleDbContext(DbContextOptions<PromotionsModuleDbContext> options) : DbContext(options), IPromotionsDbContext
    {
        public DbSet<HeroBanner> HeroBanners { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("promotions");
            builder.ApplyConfigurationsFromAssembly(typeof(PromotionsModuleDbContext).Assembly);
        }
    }
}

