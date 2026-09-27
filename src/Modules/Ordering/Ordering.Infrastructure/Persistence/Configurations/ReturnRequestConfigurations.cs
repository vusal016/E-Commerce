namespace Ordering.Infrastructure.Persistence.Configurations
{
    public sealed class ReturnRequestConfigurations : IEntityTypeConfiguration<ReturnRequest>
    {
        public void Configure(EntityTypeBuilder<ReturnRequest> builder)
        {
            builder.ToTable("return_requests");
            builder.HasKey(r => r.Id);

            builder.HasOne<OrderItem>()
                .WithMany()
                .HasForeignKey(r => r.OrderItemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
