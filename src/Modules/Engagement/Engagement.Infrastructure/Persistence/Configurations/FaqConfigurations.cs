namespace Engagement.Infrastructure.Persistence.Configurations
{
    public sealed class FaqConfigurations : IEntityTypeConfiguration<Faq>
    {
        public void Configure(EntityTypeBuilder<Faq> builder)
        {
            builder.ToTable("faqs");
            builder.HasKey(f => f.Id);
        }
    }
}