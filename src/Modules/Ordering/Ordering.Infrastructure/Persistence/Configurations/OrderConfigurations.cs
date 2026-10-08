namespace Ordering.Infrastructure.Persistence.Configurations
{
    public sealed class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("orders");
            builder.HasKey(o => o.Id);
            builder.Property<uint>("xmin").HasColumnType("xid").IsRowVersion();
            builder.HasIndex(o => o.OrderNumber).IsUnique();
            builder.HasMany(o => o.Items)
                .WithOne(i => i.Order)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(o => o.StatusHistory)
                .WithOne(h => h.Order)
                .HasForeignKey(h => h.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}




