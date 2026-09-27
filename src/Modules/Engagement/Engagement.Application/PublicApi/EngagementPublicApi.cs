namespace Engagement.Application.PublicApi;
internal sealed class EngagementPublicApi(IEngagementDbContext dbContext) : IEngagementPublicApi
{
    public async Task<ProductRatingDto> GetProductRatingAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        var rawResult = await dbContext.Reviews
            .AsNoTracking()
            .Where(r => r.ProductId == productId)
            .GroupBy(r => r.ProductId)
            .Select(g => new
            {
                ProductId = g.Key,
                Average = g.Average(r => (decimal)r.Rating),
                Count = g.Count()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (rawResult == null) return new ProductRatingDto(productId, 0m, 0);
        return new ProductRatingDto(rawResult.ProductId, Math.Round(rawResult.Average, 2), rawResult.Count);
    }

    public async Task<List<ProductRatingDto>> GetProductRatingsAsync(IEnumerable<Guid> productIds, CancellationToken cancellationToken = default)
    {
        var ids = productIds.Distinct().ToList();
        
        var rawList = await dbContext.Reviews
            .AsNoTracking()
            .Where(r => ids.Contains(r.ProductId))
            .GroupBy(r => r.ProductId)
            .Select(g => new
            {
                ProductId = g.Key,
                Average = g.Average(r => (decimal)r.Rating),
                Count = g.Count()
            })
            .ToListAsync(cancellationToken);
            
        return rawList.Select(r => new ProductRatingDto(r.ProductId, Math.Round(r.Average, 2), r.Count)).ToList();
    }
}
