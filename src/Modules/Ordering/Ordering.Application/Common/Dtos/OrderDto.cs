namespace Ordering.Application.Common.Dtos;
public sealed record OrderDto(Guid Id, string OrderNumber, string Status, decimal Total, DateTime PlacedAt, IEnumerable<OrderItemDto> Items);
