namespace Catalog.Application.Features.SearchProducts
{
    public sealed record SearchProductsQuery(
        string Query,
        int Page = 1,
        int Limit = 10) : IRequest<(SearchResponseDto Data, PaginationInfo Pagination)>;
}