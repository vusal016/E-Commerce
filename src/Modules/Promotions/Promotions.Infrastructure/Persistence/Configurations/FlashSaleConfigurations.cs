namespace Promotions.Infrastructure.Persistence.Configurations
{
    public sealed class FlashSaleConfigurations : IEntityTypeConfiguration<FlashSale>
    {
        public void Configure(EntityTypeBuilder<FlashSale> builder)
        {
            builder.ToTable("flash_sales");
            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.Items)
                   .WithOne(x => x.FlashSale)
                   .HasForeignKey(x => x.FlashSaleId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
