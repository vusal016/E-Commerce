namespace Catalog.Domain.Brand
{
    public sealed class Brand: BaseEntity
    {
        private Brand()
        {
        }
        public Brand(string name, string slug, string logoUrl, string description)
        {
            SetName(name);
            SetSlug(slug);
            SetLogoUrl(logoUrl);
            SetDescription(description);
        }
        public string Name { get; private set; }
        public string Slug { get; private set; }
        public string LogoUrl { get;private set; }
        public string Description { get;private set; }
        private void SetName(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name, "Brand name cannot be empty.");
            Name = name;
        }
        private void SetSlug(string slug)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(slug, "Brand slug cannot be empty.");
            Slug = slug;
        }
        private void SetLogoUrl(string logoUrl)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(logoUrl, "Brand logo URL cannot be empty.");
            LogoUrl = logoUrl;
        }
        private void SetDescription(string description)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(description, "Brand description cannot be empty.");
            Description = description;
        }
    }
}
