namespace Promotions.Infrastructure.Persistence.Configurations
{
    public sealed class HeroBannerConfigurations : IEntityTypeConfiguration<HeroBanner>
    {
        public void Configure(EntityTypeBuilder<HeroBanner> builder)
        {
            builder.ToTable("hero_banners");
            builder.HasKey(h => h.Id);

            builder.Property(h => h.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(h => h.Subtitle)
                .HasMaxLength(500);

            builder.Property(h => h.ImageUrl)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(h => h.MobileImageUrl)
                .HasMaxLength(1000);

            builder.Property(h => h.LinkUrl)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(h => h.ButtonText)
                .HasMaxLength(100);

            builder.Property(h => h.DisplayOrder)
                .IsRequired();

            builder.Property(h => h.IsActive)
                .IsRequired();
        }
    }
}

