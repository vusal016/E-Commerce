namespace Catalog.Application.Features.HomeCategories.Queries
{
    public sealed record GetHomeCategoriesQuery() : ICachedQuery<List<HomeCategoryDto>>
    {
        public string Key => "home-categories";
        public TimeSpan? Expiration => TimeSpan.FromMinutes(5);
    }
}