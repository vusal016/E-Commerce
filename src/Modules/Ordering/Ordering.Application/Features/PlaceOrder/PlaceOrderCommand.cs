namespace Ordering.Application.Features.PlaceOrder;

internal sealed record PlaceOrderCommand(Guid? UserId, string? SessionId) : IRequest<Guid>;
