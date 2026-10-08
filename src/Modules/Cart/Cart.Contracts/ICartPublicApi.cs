namespace Cart.Contracts
{
    public interface ICartPublicApi
    {
        Task<CartCheckoutInfoDto?> GetCartForCheckoutAsync(Guid? userId, string? sessionId, CancellationToken cancellationToken = default);
        Task ClearCartAsync(Guid? userId, string? sessionId, CancellationToken cancellationToken = default);
        Task<bool> AddItemAsync(Guid? userId, string? sessionId, Guid productVariantId, int quantity, CancellationToken cancellationToken = default);
    }
}