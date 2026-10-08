namespace Ordering.Application.Features.PlaceOrder;

public sealed record PlaceOrderCommand(Guid? UserId, string? SessionId) : IRequest<Guid>;
