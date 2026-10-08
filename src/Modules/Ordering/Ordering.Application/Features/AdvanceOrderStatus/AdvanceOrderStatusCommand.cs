namespace Ordering.Application.Features.AdvanceOrderStatus;
public sealed record AdvanceOrderStatusCommand(Guid? UserId, string OrderNumber) : IRequest<bool>;
