namespace Web.Api.Messaging.Processing
{
    internal sealed class IntegrationEventProcessorJob(InMemoryMessageQueue queue, IServiceScopeFactory scopeFactory, ILogger<IntegrationEventProcessorJob> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var integrationEvent in queue.Reader.ReadAllAsync(stoppingToken))
            {
                try
                {
                    using var scope = scopeFactory.CreateScope();
                    var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();
                    await publisher.Publish(integrationEvent, stoppingToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Integration event {EventType} processing failed", integrationEvent.GetType().Name);
                }
            }
        }
    }
}