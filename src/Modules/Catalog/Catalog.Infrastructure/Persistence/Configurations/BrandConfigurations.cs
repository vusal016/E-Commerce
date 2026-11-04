namespace Catalog.Infrastructure.Persistence.Configurations
{
    internal sealed class BrandConfigurations : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.ToTable("brands");
            builder.HasKey(b => b.Id);
        }
    }
}