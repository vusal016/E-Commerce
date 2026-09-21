namespace Promotions.Application.Features.HeroBanners.Queries
{
    public sealed record GetHeroBannersQuery() : IRequest<IReadOnlyList<HeroBannerDto>>;
}

