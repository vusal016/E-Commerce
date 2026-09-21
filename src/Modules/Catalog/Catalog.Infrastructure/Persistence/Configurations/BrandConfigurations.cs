namespace Catalog.Infrastructure.Persistence.Configurations
{
    public sealed class BrandConfigurations : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.ToTable("brands");
            builder.HasKey(b => b.Id);
        }
    }
}
