
namespace Ordering.Contracts.Events;

public sealed record OrderPlacedIntegrationEvent(Guid Id, Guid? UserId, string? SessionId, IReadOnlyList<OrderItemSnapshot> Items) : IntegrationEvent(Id);

