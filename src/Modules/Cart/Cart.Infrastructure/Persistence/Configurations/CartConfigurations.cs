namespace Cart.Infrastructure.Persistence.Configurations
{
    public sealed class CartConfigurations : IEntityTypeConfiguration<Domain.CartAggregate.Cart>
    {
        public void Configure(EntityTypeBuilder<Domain.CartAggregate.Cart> builder)
        {
            builder.ToTable("carts");
            builder.HasKey(c => c.Id);
            builder.HasMany(c => c.Items)
                .WithOne(i => i.Cart)
                .HasForeignKey(i => i.CartId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}




