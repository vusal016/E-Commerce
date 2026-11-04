namespace Engagement.Infrastructure.Persistence.Configurations
{
    internal sealed class ReviewConfigurations : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("reviews");
            builder.HasKey(r => r.Id);
            builder.Property(p => p.Rating).HasPrecision(3, 2);
        }
    }
}