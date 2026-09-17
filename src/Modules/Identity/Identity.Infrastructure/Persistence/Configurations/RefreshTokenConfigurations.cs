namespace Identity.Infrastructure.Persistence.Configurations
{
    public sealed class RefreshTokenConfigurations : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {   
            builder.ToTable("refresh_tokens");
            builder.HasIndex(x => x.Token).IsUnique();
        }
    }
}