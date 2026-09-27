namespace Identity.Infrastructure.Persistence.Configurations
{
    internal sealed class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("addresses");
            builder.HasKey(a => a.Id);
        }
    }
}
