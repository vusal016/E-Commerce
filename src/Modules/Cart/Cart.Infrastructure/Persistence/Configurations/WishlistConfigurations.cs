namespace Cart.Infrastructure.Persistence.Configurations
{
    public sealed class WishlistConfigurations : IEntityTypeConfiguration<Wishlist>
    {
        public void Configure(EntityTypeBuilder<Wishlist> builder)
        {
            builder.ToTable("wishlists");
            builder.HasKey(w => w.Id);
            builder.HasMany(w => w.Items)
                .WithOne(i => i.Wishlist)
                .HasForeignKey(i => i.WishlistId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
