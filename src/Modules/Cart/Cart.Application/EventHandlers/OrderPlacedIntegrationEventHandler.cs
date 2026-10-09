namespace Cart.Application.EventHandlers
{
    internal sealed class OrderPlacedIntegrationEventHandler(ICartDbContext dbContext)
        : IIntegrationEventHandler<OrderPlacedIntegrationEvent>
    {
        public async Task Handle(OrderPlacedIntegrationEvent integrationEvent, CancellationToken cancellationToken)
        {
            await dbContext.Carts
                .Where(c =>
                    (integrationEvent.UserId != null && c.UserId == integrationEvent.UserId) ||
                    c.SessionId == integrationEvent.SessionId)
                .ExecuteDeleteAsync(cancellationToken);
        }
    }
}