namespace Identity.Infrastructure.Persistence.Configurations
{
    public sealed class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");
        }
    }
}