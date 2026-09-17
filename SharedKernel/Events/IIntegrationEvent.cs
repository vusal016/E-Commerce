namespace SharedKernel.Events
{
    public interface IIntegrationEvent:INotification
    {
        public Guid Id { get; init; }
    }

    public abstract record IntegrationEvent(Guid Id) : IIntegrationEvent;
}