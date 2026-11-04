namespace Catalog.Contracts
{
    public interface ICatalogPublicApi
    {
        Task<IReadOnlyList<ProductSummaryDto>> GetProductSummariesAsync(IEnumerable<Guid> variantIds, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<BasketProductDto>> GetBasketProductsAsync(IEnumerable<Guid> variantIds, CancellationToken cancellationToken = default);
        Task<bool> TryReserveStockAsync(Guid variantId, int quantity, CancellationToken cancellationToken = default);
        Task ReleaseStockAsync(Guid variantId, int quantity, CancellationToken cancellationToken = default);
    }
}