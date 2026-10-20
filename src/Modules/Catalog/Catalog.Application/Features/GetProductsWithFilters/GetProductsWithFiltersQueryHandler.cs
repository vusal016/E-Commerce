namespace Catalog.Application.Features.HomeCategoriesWithFilters
{
    internal sealed class GetProductsWithFiltersQueryHandler(
        ICatalogDbContext dbContext,
        IPromotionsPublicApi promotionsApi) : IRequestHandler<GetProductsWithFiltersQuery, (List<GetProductsWithFiltersDto> Products, PaginationInfo Pagination)>
    {
        public async Task<(List<GetProductsWithFiltersDto> Products, PaginationInfo Pagination)> Handle(GetProductsWithFiltersQuery request, CancellationToken cancellationToken)
        {
            int limit = Math.Clamp(request.Limit, 1, 50);
            int page = Math.Max(request.Page, 1);
            var productsQuery = dbContext.Products
                .AsNoTracking()
                .Include(p => p.ProductImages)
                .Include(p => p.ProductVariants)
                .Where(p => p.IsActive);
            var query = from p in productsQuery
                        join c in dbContext.Categories.AsNoTracking() on p.CategoryId equals c.Id
                        select new { Product = p, Category = c };
            if (!string.IsNullOrWhiteSpace(request.Slug))
            {
                var catSlug = request.Slug.ToLower();
                query = query.Where(x => x.Category.Slug.ToLower() == catSlug);
            }
            if (!string.IsNullOrWhiteSpace(request.Size))
            {
                var size = request.Size.ToLower();
                query = query.Where(x => x.Product.ProductVariants.Any(v => v.Size.ToLower() == size));
            }
            if (!string.IsNullOrWhiteSpace(request.Color))
            {
                var color = request.Color.ToLower();
                query = query.Where(x => x.Product.ProductVariants.Any(v => v.Color.ToLower() == color));
            }
            if (request.MinPrice.HasValue || request.MaxPrice.HasValue)
            {
                query = query.Where(x => x.Product.ProductVariants.Any(v =>
                    (!request.MinPrice.HasValue || v.Price >= request.MinPrice.Value) &&
                    (!request.MaxPrice.HasValue || v.Price <= request.MaxPrice.Value)));
            }
            var sortMode = request.Sort?.ToLower() ?? "relevance";
            query = sortMode switch
            {
                "price_asc" => query.OrderBy(x => x.Product.ProductVariants.Min(v => v.Price)),
                "price_desc" => query.OrderByDescending(x => x.Product.ProductVariants.Max(v => v.Price)),
                "newest" => query.OrderByDescending(x => x.Product.CreatedAt),
                _ => query.OrderByDescending(x => x.Product.CreatedAt)
            };
            var totalRecords = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync(cancellationToken);
            var variantIds = items.SelectMany(x => x.Product.ProductVariants).Select(v => v.Id).ToList();
            var discounts = new Dictionary<Guid, ActiveDiscountDto>();
            if (variantIds.Count > 0)
            {
                var discountsList = await promotionsApi.GetActiveDiscountsAsync(variantIds, cancellationToken);
                discounts = discountsList.ToDictionary(d => d.ProductVariantId);
            }
            var dtos = items.Select(x => new GetProductsWithFiltersDto(
                x.Product.Id,
                x.Product.Name,
                x.Product.BasePrice,
                x.Category.Name,
                x.Product.ProductImages.Where(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault(),
                x.Product.ProductVariants.Select(v =>
                {
                    var discount = discounts.ContainsKey(v.Id) ? discounts[v.Id].DiscountPercentage : 0m;
                    var currentPrice = Math.Round(Math.Max(0, v.Price - (v.Price * discount / 100m)), 2);
                    return new VariantSimpleDto(
                        v.Id,
                        v.Color,
                        v.Size,
                        currentPrice,
                        v.OriginalPrice > 0 ? v.OriginalPrice : v.Price,
                        v.StockQuantity > 0);
                }).ToList()
            )).ToList();
            int totalPages = (int)Math.Ceiling(totalRecords / (double)limit);
            PaginationInfo pagination = new PaginationInfo(page, totalPages, limit, totalRecords);
            return (dtos, pagination);
        }
    }
}