namespace Engagement.Infrastructure.Persistence.Configurations
{
    public sealed class NotificationConfigurations : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("notifications");
            builder.HasKey(n => n.Id);
        }
    }
}