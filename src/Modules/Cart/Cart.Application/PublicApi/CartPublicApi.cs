namespace Cart.Application.PublicApi
{
    internal sealed class CartPublicApi(IMediator mediator) : ICartPublicApi
    {
        public async Task<CartCheckoutInfoDto?> GetCartForCheckoutAsync(Guid? userId, string? sessionId, CancellationToken cancellationToken = default)
        {
            var cartResult = await mediator.Send(new GetCartQuery(userId, sessionId), cancellationToken);
            var summaryResult = await mediator.Send(new GetCartSummaryQuery(userId, sessionId), cancellationToken);
            if (cartResult is null) return null;
            var items = cartResult.Items.Select(i => new CartCheckoutItemDto(i.ProductVariantId, i.ProductName, null, i.Quantity, i.Price)).ToList();
            return new CartCheckoutInfoDto(items, summaryResult.Subtotal, summaryResult.Discount);
        }
        public async Task ClearCartAsync(Guid? userId, string? sessionId, CancellationToken cancellationToken = default)
        {
            await mediator.Send(new ClearCartCommand(userId, sessionId), cancellationToken);
        }
        public async Task<bool> AddItemAsync(Guid? userId, string? sessionId, Guid productVariantId, int quantity, CancellationToken cancellationToken = default)
        {
            try
            {
                CartDto result = await mediator.Send(new AddItemCommand(userId, sessionId, productVariantId, quantity), cancellationToken);
                return result != null;
            }
            catch (Exception ex) when (ex is ArgumentException or KeyNotFoundException)
            {
                return false;
            }
        }
    }
}