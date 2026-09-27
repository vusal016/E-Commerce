namespace Promotions.Application.Features.FlashSales.Queries.GetActiveFlashSales;

public sealed class GetActiveFlashSalesQueryHandler(IPromotionsDbContext dbContext, ICatalogPublicApi catalogApi) : IRequestHandler<GetActiveFlashSalesQuery, IReadOnlyList<FlashSaleDto>>
{
    public async Task<IReadOnlyList<FlashSaleDto>> Handle(GetActiveFlashSalesQuery request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var activeSales = await dbContext.FlashSales
            .AsNoTracking()
            .Include(fs => fs.Items)
            .Where(fs => fs.IsActive && fs.StartsAt <= now && fs.EndsAt > now)
            .ToListAsync(cancellationToken);

        if (activeSales.Count == 0) return [];

        var variantIds = activeSales.SelectMany(fs => fs.Items).Select(i => i.ProductVariantId).Distinct();
        var products = (await catalogApi.GetProductSummariesAsync(variantIds, cancellationToken)).ToDictionary(p => p.VariantId);

        return activeSales.Select(sale => new FlashSaleDto(
            sale.Id,
            sale.Name,
            sale.StartsAt,
            sale.EndsAt,
            sale.IsActive,
            sale.Items
                .Where(item => products.ContainsKey(item.ProductVariantId))
                .Select(item => 
                {
                    var product = products[item.ProductVariantId];
                    var discountedPrice = Math.Round(Math.Max(0, product.Price - (product.Price * item.DiscountPercentage / 100m)), 2);
                    var soldPercentage = item.StockLimit > 0 ? (double)item.SoldCount / item.StockLimit * 100 : 0;

                    return new FlashSaleItemDto(
                        item.Id,
                        item.ProductVariantId,
                        product.ProductName,
                        product.PrimaryImageUrl,
                        product.Price,
                        discountedPrice,
                        item.DiscountPercentage,
                        item.StockLimit,
                        item.SoldCount,
                        soldPercentage);
                }).ToList())).ToList();
    }
}

