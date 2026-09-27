namespace Promotions.Infrastructure.Persistence.Configurations
{
    public sealed class NotifyRequestConfigurations : IEntityTypeConfiguration<NotifyRequest>
    {
        public void Configure(EntityTypeBuilder<NotifyRequest> builder)
        {
            builder.ToTable("notify_requests");
            builder.HasKey(n => n.Id);
        }
    }
}
