namespace Cart.Infrastructure.Persistence.Configurations
{
    public sealed class WishlistItemConfigurations : IEntityTypeConfiguration<WishlistItem>
    {
        public void Configure(EntityTypeBuilder<WishlistItem> builder)
        {
            builder.ToTable("wishlist_items");
            builder.HasKey(i => i.Id);
        }
    }
}