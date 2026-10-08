namespace Ordering.Application.Features.GetInvoice;

internal sealed class GetInvoiceQueryHandler(IOrderingDbContext dbContext) : IRequestHandler<GetInvoiceQuery, InvoiceDto>
{
    public async Task<InvoiceDto> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders.Include(o => o.Items).AsNoTracking().FirstOrDefaultAsync(o => o.OrderNumber == request.OrderNumber && (request.UserId == null || o.UserId == request.UserId), cancellationToken);

        if (order is null)
            throw new KeyNotFoundException("Order not found.");

        return new InvoiceDto($"INV-{order.OrderNumber}", order.OrderNumber, order.PlacedAt, order.UserId, order.ShippingAddressId, order.Items.Select(i => new InvoiceItemDto(i.ProductNameSnapshot, i.PriceSnapshot, i.Quantity, i.PriceSnapshot * i.Quantity)), order.Subtotal, order.ShippingCost, order.Tax, order.Discount, order.Total);
    }
}
