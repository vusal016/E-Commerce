namespace Ordering.Application.Features.GetOrders;
public sealed record GetOrdersQuery(Guid? UserId, string? Status, string? Search, int Page) : IRequest<(IEnumerable<OrderDto> Data, PaginationInfo Pagination)>;
