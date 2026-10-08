namespace Identity.Infrastructure.Persistence.IdentityData
{
    public sealed class IdentityModuleDbContext(DbContextOptions<IdentityModuleDbContext> options) : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options), IIdentityDbContext
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Address> Addresses { get; set; }

        
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
                        foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var idProperty = entityType.FindProperty("Id");
                if (idProperty != null && idProperty.ClrType == typeof(Guid))
                {
                    idProperty.ValueGenerated = Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.Never;
                }
            }
            builder.HasDefaultSchema("identity");
            builder.ApplyConfigurationsFromAssembly(typeof(IdentityModuleDbContext).Assembly);
        }
    }
}








