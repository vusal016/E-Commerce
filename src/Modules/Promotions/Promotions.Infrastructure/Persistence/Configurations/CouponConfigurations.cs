namespace Promotions.Infrastructure.Persistence.Configurations
{
    public sealed class CouponConfigurations : IEntityTypeConfiguration<Coupon>
    {
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            builder.ToTable("coupons");
            builder.HasKey(c => c.Id);
            builder.HasIndex(c => c.Code).IsUnique();
        }
    }
}


