namespace Catalog.Application.Features.HomeCategoriesWithFilters
{
    public sealed record GetProductsWithFiltersQuery(
        string? Sort,
        string? Slug,
        string? Size,
        string? Color,
        decimal? MinPrice,
        decimal? MaxPrice,
        int Page,
        int Limit) : IRequest<(List<GetProductsWithFiltersDto> Products, PaginationInfo Pagination)>;
}