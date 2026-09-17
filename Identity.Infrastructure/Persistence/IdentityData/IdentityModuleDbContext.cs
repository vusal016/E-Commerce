namespace Identity.Infrastructure.Persistence.IdentityData
{
    public sealed class IdentityModuleDbContext(DbContextOptions<IdentityModuleDbContext> options) : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options), IIdentityDbContext
    {
        public DbSet<User> Users { set; get; }
        public DbSet<RefreshToken> RefreshTokens { set; get; }

        override protected void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("identity");
            builder.ApplyConfigurationsFromAssembly(typeof(IdentityModuleDbContext).Assembly);
        }
    }
}