namespace Catalog.Application.Features.GetBrandStoreBySlug
{
    public sealed record GetBrandStoreBySlugQuery(string Slug) : ICachedQuery<BrandStoreDto>
    {
        public string Key => $"BrandStore_{Slug}";
        public TimeSpan? Expiration => TimeSpan.FromMinutes(30);
    }
}
