
namespace SharedKernel.Events;

public interface IIntegrationEventHandler<TIntegrationEvent> : INotificationHandler<TIntegrationEvent> 
    where TIntegrationEvent : IIntegrationEvent
{
}

