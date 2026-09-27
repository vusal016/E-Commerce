namespace Cart.Infrastructure.Persistence.Configurations
{
    public sealed class CartItemConfigurations : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.ToTable("cart_items");
            builder.HasKey(i => i.Id);
        }
    }
}
