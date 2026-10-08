namespace Catalog.Application.PublicApi
{
    internal sealed class CatalogPublicApi(ICatalogDbContext catalogDbContext) : ICatalogPublicApi
    {
        public async Task<IReadOnlyList<ProductSummaryDto>> GetProductSummariesAsync(IEnumerable<Guid> variantIds, CancellationToken cancellationToken = default)
        {
            var variantIdList = variantIds.ToList();
            if (variantIdList.Count == 0) return [];
            return await catalogDbContext.Products
                .AsNoTracking()
                .SelectMany(p => p.ProductVariants, (p, v) => new { Product = p, Variant = v })
                .Where(x => variantIdList.Contains(x.Variant.Id))
                .Select(x => new ProductSummaryDto(
                    x.Product.Id,
                    x.Variant.Id,
                    x.Product.Name,
                    x.Product.ProductImages.Where(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault(),
                    x.Variant.Price,
                    x.Variant.StockQuantity
                ))
                .ToListAsync(cancellationToken);
        }
        public async Task<IReadOnlyList<BasketProductDto>> GetBasketProductsAsync(IEnumerable<Guid> variantIds, CancellationToken cancellationToken = default)
        {
            var variantIdList = variantIds.ToList();
            if (variantIdList.Count == 0) return [];
            return await catalogDbContext.Products
                .AsNoTracking()
                .SelectMany(p => p.ProductVariants, (p, v) => new { Product = p, Variant = v })
                .Where(x => variantIdList.Contains(x.Variant.Id))
                .Select(x => new BasketProductDto(
                    x.Variant.Id,
                    x.Product.Name,
                    x.Variant.Color,
                    x.Variant.Size,
                    x.Product.ProductImages.Where(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault(),
                    x.Variant.Price,
                    x.Variant.OriginalPrice,
                    x.Variant.StockQuantity
                ))
                .ToListAsync(cancellationToken);
        }
    }
}