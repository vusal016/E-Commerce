namespace Ordering.Application.Features.GetOrderDetail;
public sealed record GetOrderDetailQuery(Guid? UserId, string OrderNumber) : IRequest<OrderDetailDto>;
