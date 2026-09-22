using SharedKernel.Cache;

namespace Promotions.Application.Features.HeroBanners.Queries
{
    public sealed record GetHeroBannersQuery() : ICachedQuery<List<HeroBannerDto>>
    {
        public string Key => "hero-banners";

        public TimeSpan? Expiration => TimeSpan.FromMinutes(2);
    }
}

