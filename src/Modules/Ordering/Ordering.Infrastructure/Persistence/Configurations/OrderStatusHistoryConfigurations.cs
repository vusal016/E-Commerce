namespace Ordering.Infrastructure.Persistence.Configurations
{
    public sealed class OrderStatusHistoryConfigurations : IEntityTypeConfiguration<OrderStatusHistory>
    {
        public void Configure(EntityTypeBuilder<OrderStatusHistory> builder)
        {
            builder.ToTable("order_status_history");
            builder.HasKey(h => h.Id);
        }
    }
}
