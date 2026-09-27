namespace Promotions.Infrastructure.Persistence.PromotionsData
{
    public sealed class PromotionsModuleDbContext(DbContextOptions<PromotionsModuleDbContext> options) : DbContext(options), IPromotionsDbContext
    {
        public DbSet<HeroBanner> HeroBanners { get; set; }
        public DbSet<FlashSale> FlashSales { get; set; }
        public DbSet<FlashSaleItem> FlashSaleItems { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<NotifyRequest> NotifyRequests { get; set; }

        
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("promotions");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PromotionsModuleDbContext).Assembly);
        }
    }
}




