
namespace Cart.Application.EventHandlers;

internal sealed class OrderPlacedIntegrationEventHandler(ICartDbContext dbContext) : IIntegrationEventHandler<OrderPlacedIntegrationEvent>
{
    public async Task Handle(OrderPlacedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var cart = await dbContext.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => (notification.UserId != null && c.UserId == notification.UserId) || (notification.SessionId != null && c.SessionId == notification.SessionId), cancellationToken);

        if (cart != null)
        {
            dbContext.Carts.Remove(cart);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}

