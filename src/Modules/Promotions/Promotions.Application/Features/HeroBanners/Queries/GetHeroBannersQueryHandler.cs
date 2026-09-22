namespace Promotions.Application.Features.HeroBanners.Queries
{
    public sealed class GetHeroBannersQueryHandler(IPromotionsDbContext dbContext, IMapper mapper) : IRequestHandler<GetHeroBannersQuery, List<HeroBannerDto>>
    {
        public async Task<List<HeroBannerDto>> Handle(GetHeroBannersQuery request, CancellationToken cancellationToken)
        {
            var banners = await dbContext.HeroBanners
                .AsNoTracking()
                .Where(b => b.IsActive)
                .OrderBy(b => b.DisplayOrder)
                .ToListAsync(cancellationToken);

            return mapper.Map<List<HeroBannerDto>>(banners);
        }
    }
}

