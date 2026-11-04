namespace Ordering.Infrastructure.Persistence.Configurations
{
    internal sealed class CheckoutSessionConfigurations : IEntityTypeConfiguration<CheckoutSession>
    {
        public void Configure(EntityTypeBuilder<CheckoutSession> builder)
        {
            builder.ToTable("checkout_sessions");
            builder.HasKey(c => c.Id);
            builder.HasIndex(c => c.UserId).IsUnique().HasFilter("\"UserId\" IS NOT NULL AND \"IsCompleted\" = false");
            builder.HasIndex(c => c.SessionId).IsUnique().HasFilter("\"UserId\" IS NULL AND \"IsCompleted\" = false");
            builder.Property(c => c.Version).IsRowVersion();
        }
    }
}