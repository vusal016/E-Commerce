namespace Cart.Contracts
{
    public sealed record CartCheckoutInfoDto(IReadOnlyList<CartCheckoutItemDto> Items, decimal Subtotal, decimal CartDiscount);
}