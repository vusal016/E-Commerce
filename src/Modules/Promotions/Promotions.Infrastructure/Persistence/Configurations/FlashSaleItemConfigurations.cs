namespace Promotions.Infrastructure.Persistence.Configurations
{
    public sealed class FlashSaleItemConfigurations : IEntityTypeConfiguration<FlashSaleItem>
    {
        public void Configure(EntityTypeBuilder<FlashSaleItem> builder)
        {
            builder.ToTable("flash_sale_items");
            builder.HasKey(x => x.Id);
        }
    }
}


