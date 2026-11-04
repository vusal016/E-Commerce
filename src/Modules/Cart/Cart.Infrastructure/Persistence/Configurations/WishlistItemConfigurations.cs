namespace Cart.Infrastructure.Persistence.Configurations
{
    internal sealed class WishlistItemConfigurations : IEntityTypeConfiguration<WishlistItem>
    {
        public void Configure(EntityTypeBuilder<WishlistItem> builder)
        {
            builder.ToTable("wishlist_items");
            builder.HasKey(i => i.Id);
            builder.HasIndex(i => new { i.WishlistId, i.ProductVariantId }).IsUnique();
        }
    }
}