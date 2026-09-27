namespace Engagement.Infrastructure.Persistence.Configurations
{
    public sealed class SupportArticleConfigurations : IEntityTypeConfiguration<SupportArticle>
    {
        public void Configure(EntityTypeBuilder<SupportArticle> builder)
        {
            builder.ToTable("support_articles");
            builder.HasKey(s => s.Id);
        }
    }
}
