namespace Catalog.Application.EventHandlers
{
    internal sealed class OrderPlacedIntegrationEventHandler(ICatalogDbContext dbContext) : IIntegrationEventHandler<OrderPlacedIntegrationEvent>
    {
        public Task Handle(OrderPlacedIntegrationEvent integrationEvent, CancellationToken cancellationToken)
        {
            _ = dbContext;
            _ = integrationEvent;
            _ = cancellationToken;
            return Task.CompletedTask;
        }
    }
}