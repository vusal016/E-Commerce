namespace Engagement.Contracts;

public interface IEngagementPublicApi
{
    Task<ProductRatingDto> GetProductRatingAsync(Guid productId, CancellationToken cancellationToken = default);
    Task<List<ProductRatingDto>> GetProductRatingsAsync(IEnumerable<Guid> productIds, CancellationToken cancellationToken = default);
}
