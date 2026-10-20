namespace Ordering.Application.Features.PlaceOrder
{
    internal sealed class PlaceOrderCommandHandler(IOrderingDbContext orderingDb, ICartPublicApi cartApi, ICatalogPublicApi catalogApi, IEventBus eventBus) : IRequestHandler<PlaceOrderCommand, Guid>
    {
        public async Task<Guid> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
        {
            var session = await orderingDb.CheckoutSessions.ForOwner(request.UserId, request.SessionId).FirstOrDefaultAsync(cancellationToken);
            if (session is null)
                throw new KeyNotFoundException("Checkout session not found.");
            if (string.IsNullOrWhiteSpace(session.Email) ||
                string.IsNullOrWhiteSpace(session.Address) ||
                string.IsNullOrWhiteSpace(session.ShippingMethod) ||
                string.IsNullOrWhiteSpace(session.PaymentMethod))
            {
                throw new InvalidOperationException("Shipping and payment information must be completed.");
            }
            var cartInfo = await cartApi.GetCartForCheckoutAsync(request.UserId, request.SessionId, cancellationToken);
            if (cartInfo is null || cartInfo.Items.Count == 0)
                throw new InvalidOperationException("Cart is empty.");
            var variantIds = cartInfo.Items.Select(i => i.ProductVariantId).Distinct().ToList();
            var basketProducts = await catalogApi.GetBasketProductsAsync(variantIds, cancellationToken);
            var reservedItems = new List<(Guid VariantId, int Quantity)>();
            Order order;
            try
            {
                foreach (var cartItem in cartInfo.Items)
                {
                    var product = basketProducts.FirstOrDefault(p => p.VariantId == cartItem.ProductVariantId);
                    if (product is null)
                        throw new InvalidOperationException($"Product {cartItem.ProductName} not found.");
                    if (product.StockQuantity < cartItem.Quantity)
                        throw new ArgumentException($"Insufficient stock for {cartItem.ProductName}. Available: {product.StockQuantity}, Requested: {cartItem.Quantity}");
                    var reserved = await catalogApi.TryReserveStockAsync(cartItem.ProductVariantId, cartItem.Quantity, cancellationToken);
                    if (!reserved)
                        throw new InvalidOperationException($"Insufficient stock for {cartItem.ProductName}.");
                    reservedItems.Add((cartItem.ProductVariantId, cartItem.Quantity));
                }
                decimal totalPrice = cartInfo.Subtotal - cartInfo.CartDiscount + session.ShippingPrice;
                totalPrice = totalPrice < 0 ? 0 : totalPrice;
                var orderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpperInvariant()}";
                // TODO: real address/payment IDs
                order = new Order(request.UserId, orderNumber, "Pending", cartInfo.Subtotal, session.ShippingPrice, 0, cartInfo.CartDiscount, totalPrice, Guid.NewGuid(), Guid.NewGuid(), session.ShippingMethod ?? "Standard", null, null, DateTime.UtcNow, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(5));
                foreach (var ci in cartInfo.Items)
                {
                    order.AddItem(ci.ProductVariantId, ci.ProductName, ci.Price, ci.Quantity);
                }
                session.MarkAsCompleted();
                orderingDb.Orders.Add(order);
                await orderingDb.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                foreach (var reservedItem in reservedItems)
                {
                    await catalogApi.ReleaseStockAsync(reservedItem.VariantId, reservedItem.Quantity, cancellationToken);
                }
                throw;
            }
            var eventItems = cartInfo.Items.Select(i => new OrderItemSnapshot(i.ProductVariantId, i.Quantity)).ToList();
            var integrationEvent = new OrderPlacedIntegrationEvent(order.Id, request.UserId, request.SessionId, eventItems);
            await eventBus.PublishAsync(integrationEvent, cancellationToken);
            return order.Id;
        }
    }
}