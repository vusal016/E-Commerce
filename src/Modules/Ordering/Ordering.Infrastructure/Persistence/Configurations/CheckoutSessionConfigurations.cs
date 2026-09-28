namespace Ordering.Infrastructure.Persistence.Configurations
{
    public sealed class CheckoutSessionConfigurations : IEntityTypeConfiguration<CheckoutSession>
    {
        public void Configure(EntityTypeBuilder<CheckoutSession> builder)
        {
            builder.ToTable("checkout_sessions");
            builder.HasKey(c => c.Id);
            builder.HasIndex(c => c.SessionId);
            builder.HasIndex(c => c.UserId);
        }
    }
}
