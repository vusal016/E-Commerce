namespace Web.Api.Messaging.Processing
{
    internal sealed class IntegrationEventProcessorJob(InMemoryMessageQueue queue, IServiceScopeFactory scopeFactory) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var integrationEvent in queue.Reader.ReadAllAsync(stoppingToken))
            {
                using var scope = scopeFactory.CreateScope();
                var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();
                await publisher.Publish(integrationEvent, stoppingToken);
            }
        }
    }
}
