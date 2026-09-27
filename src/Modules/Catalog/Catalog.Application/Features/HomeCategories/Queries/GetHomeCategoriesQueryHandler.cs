namespace Catalog.Application.Features.HomeCategories.Queries
{
    public sealed class GetHomeCategoriesQueryHandler(ICatalogDbContext dbContext, IMapper mapper) : IRequestHandler<GetHomeCategoriesQuery, List<HomeCategoryDto>>
    {
        public async Task<List<HomeCategoryDto>> Handle(GetHomeCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await dbContext.Categories
                .AsNoTracking()
                .Where(c => c.ParentCategoryId == null)
                .ToListAsync(cancellationToken);

            return mapper.Map<List<HomeCategoryDto>>(categories);
        }
    }
}
