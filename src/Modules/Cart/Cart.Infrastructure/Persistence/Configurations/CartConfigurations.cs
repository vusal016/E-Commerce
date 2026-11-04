namespace Cart.Infrastructure.Persistence.Configurations
{
    internal sealed class CartConfigurations : IEntityTypeConfiguration<CartEntity>
    {
        public void Configure(EntityTypeBuilder<CartEntity> builder)
        {
            builder.ToTable("carts");
            builder.HasKey(c => c.Id);
            builder.HasIndex(c => c.UserId).IsUnique().HasFilter("\"UserId\" IS NOT NULL");
            builder.HasIndex(c => c.SessionId).IsUnique().HasFilter("\"UserId\" IS NULL");
            builder.HasMany(c => c.Items)
                .WithOne(i => i.Cart)
                .HasForeignKey(i => i.CartId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}