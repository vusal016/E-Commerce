namespace Engagement.Infrastructure.Persistence.EngagementData
{
    public sealed class EngagementModuleDbContext(DbContextOptions<EngagementModuleDbContext> options) : DbContext(options), IEngagementDbContext
    {
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Faq> Faqs { get; set; }
        public DbSet<SupportArticle> SupportArticles { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("engagement");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EngagementModuleDbContext).Assembly);
        }
    }
}
    



