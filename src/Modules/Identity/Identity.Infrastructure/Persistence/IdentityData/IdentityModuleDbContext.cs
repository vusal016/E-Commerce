using Identity.Domain.PaymentMethod;

namespace Identity.Infrastructure.Persistence.IdentityData
{
    public sealed class IdentityModuleDbContext(DbContextOptions<IdentityModuleDbContext> options) : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options), IIdentityDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("identity");
            builder.ApplyConfigurationsFromAssembly(typeof(IdentityModuleDbContext).Assembly);
        }
    }
}