namespace Cart.Contracts;

public interface ICartPublicApi
{
    Task<CartCheckoutInfoDto?> GetCartForCheckoutAsync(Guid? userId, string? sessionId, CancellationToken cancellationToken = default);
}

public sealed record CartCheckoutInfoDto(IReadOnlyList<CartCheckoutItemDto> Items, decimal Subtotal, decimal CartDiscount);

public sealed record CartCheckoutItemDto(Guid ProductVariantId, string ProductName, string? VariantName, int Quantity, decimal Price);
