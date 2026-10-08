namespace Ordering.Infrastructure.Persistence.OrderingData
{
    public sealed class OrderingModuleDbContext(DbContextOptions<OrderingModuleDbContext> options) : DbContext(options), IOrderingDbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }
        public DbSet<ReturnRequest> ReturnRequests { get; set; }
        public DbSet<CheckoutSession> CheckoutSessions { get; set; }

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
            modelBuilder.HasDefaultSchema("ordering");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderingModuleDbContext).Assembly);
        }
    }
}






