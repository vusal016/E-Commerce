namespace Ordering.Infrastructure.Persistence.Configurations
{
    public sealed class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("order_items");
            builder.HasKey(i => i.Id);
        }
    }
}


