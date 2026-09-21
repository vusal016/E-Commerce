namespace Catalog.Infrastructure.Persistence.Configurations
{
    public sealed class CategoryConfigurations : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
           builder.ToTable("categories");
            builder.HasKey(c => c.Id);

            builder.HasOne<Category>()
                .WithMany()
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}