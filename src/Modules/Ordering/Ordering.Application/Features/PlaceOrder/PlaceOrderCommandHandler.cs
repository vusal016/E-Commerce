namespace Ordering.Application.Features.PlaceOrder;

internal sealed class PlaceOrderCommandHandler(IOrderingDbContext orderingDb, ICartPublicApi cartApi, IEventBus eventBus) : IRequestHandler<PlaceOrderCommand, Guid>
{
    public async Task<Guid> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {
        var session = await orderingDb.CheckoutSessions.FirstOrDefaultAsync(cs => (request.UserId != null && cs.UserId == request.UserId) || (request.SessionId != null && cs.SessionId == request.SessionId), cancellationToken);
            
        if (session is null) throw new KeyNotFoundException("Checkout session not found.");
        if (session.IsCompleted) throw new InvalidOperationException("Checkout session is already completed.");
        
        var cartInfo = await cartApi.GetCartForCheckoutAsync(request.UserId, request.SessionId, cancellationToken);
        if (cartInfo is null || cartInfo.Items.Count == 0) throw new InvalidOperationException("Cart is empty.");

        decimal totalPrice = cartInfo.Subtotal - cartInfo.CartDiscount + session.ShippingPrice;
        totalPrice = totalPrice < 0 ? 0 : totalPrice;

        var orderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}";
        
        var order = new Order(request.UserId, orderNumber, "Pending", cartInfo.Subtotal, session.ShippingPrice, 0, cartInfo.CartDiscount, totalPrice, Guid.NewGuid(), Guid.NewGuid(), session.ShippingMethod ?? "Standard", null, null, DateTime.UtcNow, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(5));

        foreach (var ci in cartInfo.Items)
        {
            order.AddItem(ci.ProductVariantId, ci.ProductName, ci.Price, ci.Quantity);
        }

        session.MarkAsCompleted(); 
        
        orderingDb.Orders.Add(order);
        await orderingDb.SaveChangesAsync(cancellationToken);

        var eventItems = cartInfo.Items.Select(i => new OrderItemSnapshot(i.ProductVariantId, i.Quantity)).ToList();
            
        var integrationEvent = new OrderPlacedIntegrationEvent(order.Id, request.UserId, request.SessionId, eventItems);
        
        await eventBus.PublishAsync(integrationEvent, cancellationToken);

        return order.Id;
    }
}
