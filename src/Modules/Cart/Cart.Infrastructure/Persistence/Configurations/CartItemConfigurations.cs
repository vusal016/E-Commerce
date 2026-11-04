namespace Cart.Infrastructure.Persistence.Configurations
{
    internal sealed class CartItemConfigurations : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.ToTable("cart_items");
            builder.HasKey(i => i.Id);
            builder.HasIndex(i => new { i.CartId, i.ProductVariantId }).IsUnique();
        }
    }
}