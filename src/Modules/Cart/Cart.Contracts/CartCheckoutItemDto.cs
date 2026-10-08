namespace Cart.Contracts
{
    public sealed record CartCheckoutItemDto(Guid ProductVariantId, string ProductName, string? VariantName, int Quantity, decimal Price);
}