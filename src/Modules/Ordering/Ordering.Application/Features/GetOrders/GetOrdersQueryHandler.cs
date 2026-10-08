namespace Ordering.Application.Features.GetOrders;

internal sealed class GetOrdersQueryHandler(IOrderingDbContext dbContext) : IRequestHandler<GetOrdersQuery, (IEnumerable<OrderDto> Data, PaginationInfo Pagination)>
{
    public async Task<(IEnumerable<OrderDto> Data, PaginationInfo Pagination)> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Orders.Include(o => o.Items).AsNoTracking()
            .Where(o => 
                (request.UserId == null || o.UserId == request.UserId) &&
                (string.IsNullOrWhiteSpace(request.Status) || request.Status.ToLower() == "all" || o.Status.ToLower() == request.Status.ToLower()) &&
                (string.IsNullOrWhiteSpace(request.Search) || o.OrderNumber.ToLower().Contains(request.Search.ToLower()) || o.Items.Any(i => i.ProductNameSnapshot.ToLower().Contains(request.Search.ToLower()))))
            .OrderByDescending(o => o.PlacedAt);

        var totalRecords = await query.CountAsync(cancellationToken);
        int limit = 10;

        var orders = await query
            .Skip((request.Page - 1) * limit)
            .Take(limit)
            .Select(o => new OrderDto(o.Id, o.OrderNumber, o.Status, o.Total, o.PlacedAt, o.Items.Select(i => new OrderItemDto(i.Id, i.ProductVariantId, i.ProductNameSnapshot, i.PriceSnapshot, i.Quantity))))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling(totalRecords / (double)limit);
        var pagination = new PaginationInfo(request.Page, totalPages, limit, totalRecords);

        return (orders, pagination);
    }
}
