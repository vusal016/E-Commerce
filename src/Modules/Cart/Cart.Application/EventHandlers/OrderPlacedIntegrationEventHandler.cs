namespace Cart.Application.EventHandlers
{
    internal sealed class OrderPlacedIntegrationEventHandler(ICartDbContext dbContext)
        : IIntegrationEventHandler<OrderPlacedIntegrationEvent>
    {
        public async Task Handle(OrderPlacedIntegrationEvent integrationEvent, CancellationToken cancellationToken)
        {
            await dbContext.Carts
                .ForOwner(integrationEvent.UserId, integrationEvent.SessionId)
                .ExecuteDeleteAsync(cancellationToken);
        }
    }
}