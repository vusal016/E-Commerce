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
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var idProperty = entityType.FindProperty("Id");
                if (idProperty != null && idProperty.ClrType == typeof(Guid))
                {
                    idProperty.ValueGenerated = Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.Never;
                }
            }
            modelBuilder.HasDefaultSchema("engagement");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EngagementModuleDbContext).Assembly);
        }
    }
}