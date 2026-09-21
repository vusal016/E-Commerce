namespace Identity.Infrastructure.Persistence.Configurations
{
    public sealed class PaymentMethodConfigurations : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> builder)
        {
            builder.ToTable("payment_methods");
            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(pm => pm.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}