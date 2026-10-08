namespace Ordering.Application.Features.GetOrderDetail;

internal sealed class GetOrderDetailQueryHandler(IOrderingDbContext dbContext) : IRequestHandler<GetOrderDetailQuery, OrderDetailDto>
{
    public async Task<OrderDetailDto> Handle(GetOrderDetailQuery request, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders.Include(o => o.Items).Include(o => o.StatusHistory).AsNoTracking().FirstOrDefaultAsync(o => o.OrderNumber == request.OrderNumber && (request.UserId == null || o.UserId == request.UserId), cancellationToken);

        if (order is null)
            throw new KeyNotFoundException("Order not found.");

        return new OrderDetailDto(order.Id, order.OrderNumber, order.Status, order.PlacedAt, order.EstimatedDeliveryStart, order.EstimatedDeliveryEnd, order.ShippingMethod, order.TrackingNumber, order.Carrier, order.ShippingAddressId, order.PaymentMethodId, order.Subtotal, order.ShippingCost, order.Tax, order.Discount, order.Total, order.Items.Select(i => new OrderItemDto(i.Id, i.ProductVariantId, i.ProductNameSnapshot, i.PriceSnapshot, i.Quantity)), order.StatusHistory.Select(sh => new OrderStatusHistoryDto(sh.Id, sh.Status, sh.ChangedAt)));
    }
}
