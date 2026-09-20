namespace Web.Api.Messaging.Processing
{
    internal sealed class IntegrationEventProcessorJob(InMemoryMessageQueue queue,IPublisher publisher) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var integrationEvent in queue.Reader.ReadAllAsync(stoppingToken))
            {
                await publisher.Publish(integrationEvent, stoppingToken);
            }
        }
    }
}