namespace Ordering.Application.Features.GetOrderTracking;

internal sealed class GetOrderTrackingQueryHandler(IOrderingDbContext dbContext) : IRequestHandler<GetOrderTrackingQuery, OrderTrackingDto>
{
    public async Task<OrderTrackingDto> Handle(GetOrderTrackingQuery request, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders.Include(o => o.StatusHistory).AsNoTracking().FirstOrDefaultAsync(o => o.OrderNumber == request.OrderNumber && (request.UserId == null || o.UserId == request.UserId), cancellationToken);

        if (order is null)
            throw new KeyNotFoundException("Order not found.");

        return new OrderTrackingDto(order.OrderNumber, order.Status, order.TrackingNumber, order.Carrier, order.EstimatedDeliveryStart, order.EstimatedDeliveryEnd, order.StatusHistory.OrderBy(sh => sh.ChangedAt).Select(sh => new OrderStatusHistoryDto(sh.Id, sh.Status, sh.ChangedAt)));
    }
}
