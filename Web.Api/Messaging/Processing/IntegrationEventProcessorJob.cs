namespace Web.Api.Messaging.Processing
{
    internal class IntegrationEventProcessorJob(InMemoryMessageQueue queue,IPublisher puplisher) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var integrationEvent in queue.Reader.ReadAllAsync(stoppingToken))
            {
                await puplisher.Publish(integrationEvent, stoppingToken);
            }
        }
    }
}