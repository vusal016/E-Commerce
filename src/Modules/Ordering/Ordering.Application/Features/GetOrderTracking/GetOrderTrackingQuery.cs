namespace Ordering.Application.Features.GetOrderTracking;
public sealed record GetOrderTrackingQuery(Guid? UserId, string OrderNumber) : IRequest<OrderTrackingDto>;
