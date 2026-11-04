namespace Identity.Infrastructure.Persistence.Configurations
{
    internal sealed class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");
        }
    }
}