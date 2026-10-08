namespace Catalog.Application.Features.GetProductById
{
    public sealed class GetProductByIdQueryHandler(
        ICatalogDbContext catalogDbContext,
        IEngagementPublicApi engagementApi,
        IPromotionsPublicApi promotionsApi) : IRequestHandler<GetProductByIdQuery, GetProductByIdDto>
    {
        public async Task<GetProductByIdDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await catalogDbContext.Products
                .AsNoTracking()
                .Include(p => p.ProductImages)
                .Include(p => p.ProductVariants)
                .FirstOrDefaultAsync(p => p.Id == request.Id && p.IsActive, cancellationToken);
            if (product == null)
            {
                throw new KeyNotFoundException("Product not found.");
            }
            var relatedProducts = await catalogDbContext.Products
                .AsNoTracking()
                .Include(p => p.ProductImages)
                .Where(p => p.CategoryId == product.CategoryId && p.Id != product.Id && p.IsActive)
                .Take(4)
                .Select(p => new RelatedProductDto(
                    p.Id,
                    p.Name,
                    p.BasePrice,
                    p.ProductImages.Where(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault()
                ))
                .ToListAsync(cancellationToken);
            // Fetch Rating from Engagement module
            var rating = await engagementApi.GetProductRatingAsync(product.Id, cancellationToken);
            // Fetch Discounts from Promotions module
            var variantIds = product.ProductVariants.Select(v => v.Id).ToList();
            var discounts = (await promotionsApi.GetActiveDiscountsAsync(variantIds, cancellationToken))
                .ToDictionary(d => d.ProductVariantId);
            return new GetProductByIdDto(
                product.Id,
                product.Name,
                product.Description,
                product.BasePrice,
                rating.RatingAverage,
                rating.ReviewCount,
                product.ProductImages.Select(i => new ProductImageDto(i.Id, i.ImageUrl, i.IsPrimary)).ToList(),
                product.ProductVariants.Select(v =>
                {
                    var discount = discounts.ContainsKey(v.Id) ? discounts[v.Id].DiscountPercentage : 0m;
                    var currentPrice = Math.Round(Math.Max(0, v.Price - (v.Price * discount / 100m)), 2);
                    return new ProductVariantDto(v.Id, v.Color, v.Size, v.Sku, currentPrice, v.OriginalPrice > 0 ? v.OriginalPrice : v.Price, v.StockQuantity);
                }).ToList(),
                relatedProducts
            );
        }
    }
}