namespace Ordering.Application.Features.Reorder;
public sealed record ReorderCommand(Guid? UserId, string? SessionId, Guid OrderId) : IRequest<ReorderResultDto>;
