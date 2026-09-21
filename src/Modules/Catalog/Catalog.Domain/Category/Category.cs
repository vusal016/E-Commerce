namespace Catalog.Domain.Catalog
{
    public sealed class Category:BaseEntity
    {
        private Category()
        {
        }
        public Category(string name, string slug, Guid? parentCategoryId=null)
        {
            SetName(name);
            SetSlug(slug);
            ParentCategoryId = parentCategoryId;
        }

        public string Name { get;private set; }
        public string Slug { get; private set; }
        public Guid? ParentCategoryId { get;private set; }

        private void SetName(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name, "Category name cannot be empty.");
            Name = name;
        }
        private void SetSlug(string slug)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(slug, "Category slug cannot be empty.");
            Slug = slug;
        }
    }
}