namespace Catalog.Contracts
{
    public interface ICatalogPublicApi
    {
        Task<IReadOnlyList<ProductSummaryDto>> GetProductSummariesAsync(IEnumerable<Guid> variantIds, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<BasketProductDto>> GetBasketProductsAsync(IEnumerable<Guid> variantIds, CancellationToken cancellationToken = default);
    }
}