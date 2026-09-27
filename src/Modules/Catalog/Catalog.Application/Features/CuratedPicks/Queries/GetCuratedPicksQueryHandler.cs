namespace Catalog.Application.Features.CuratedPicks.Queries
{
    public sealed class GetCuratedPicksQueryHandler(ICatalogDbContext catalogDbContext) : IRequestHandler<GetCuratedPicksQuery, List<CuratedPickDto>>
    {
        public async Task<List<CuratedPickDto>> Handle(GetCuratedPicksQuery request, CancellationToken cancellationToken)
        {
            var curatedPicks = await catalogDbContext.Products
                .Where(p => p.IsActive && p.ProductTags.Any(t => t.TagType == Catalog.Domain.Enums.TagType.Featured))
                .Take(10)
                .Select(p => new CuratedPickDto(
                    p.Id,
                    p.Name,
                    p.Description,
                    p.BasePrice))
                .ToListAsync(cancellationToken);

            return curatedPicks;
        }
    }
}
