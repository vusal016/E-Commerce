namespace Cart.Infrastructure.Persistence.CartData
{
    public sealed class CartModuleDbContext(DbContextOptions<CartModuleDbContext> options) : DbContext(options), ICartDbContext
    {
        public DbSet<Domain.CartAggregate.Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }

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
            modelBuilder.HasDefaultSchema("cart");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CartModuleDbContext).Assembly);
        }
    }
}