namespace Catalog.Application.Features.SearchProducts;

public sealed class SearchProductsQueryHandler(ICatalogDbContext catalogDbContext) : IRequestHandler<SearchProductsQuery, (SearchResponseDto Data, PaginationInfo Pagination)>
{
    public async Task<(SearchResponseDto Data, PaginationInfo Pagination)> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
    {
        var query = request.Query.ToLower();
        
        var productsQuery = from p in catalogDbContext.Products
                            join b in catalogDbContext.Brands on p.BrandId equals b.Id
                            where p.IsActive && b.IsActive && (p.Name.ToLower().Contains(query) || b.Name.ToLower().Contains(query))
                            select new { Product = p, BrandName = b.Name };
                            
        var totalRecords = await productsQuery.CountAsync(cancellationToken);

        var products = await productsQuery
            .Skip((request.Page - 1) * request.Limit)
            .Take(request.Limit)
            .Select(x => new SearchProductDto(
                x.Product.Id,
                x.Product.Name,
                x.Product.BasePrice,
                x.Product.ProductImages.FirstOrDefault(i => i.IsPrimary) != null 
                    ? x.Product.ProductImages.FirstOrDefault(i => i.IsPrimary)!.ImageUrl 
                    : null,
                x.BrandName))
            .ToListAsync(cancellationToken);

        string? suggestedQuery = null;
        if (products.Count == 0 && request.Page == 1)
        {
            var similarBrand = await catalogDbContext.Brands
                .Where(b => b.IsActive && b.Name.ToLower().Contains(query.Substring(0, Math.Min(query.Length, 3))))
                .FirstOrDefaultAsync(cancellationToken);
                
            if (similarBrand != null)
            {
                suggestedQuery = similarBrand.Name;
            }
        }

        var totalPages = (int)Math.Ceiling(totalRecords / (double)request.Limit);
        var pagination = new PaginationInfo(request.Page, totalPages, request.Limit, totalRecords);

        return (new SearchResponseDto(products, suggestedQuery), pagination);
    }
}
