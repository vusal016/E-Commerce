namespace Ordering.Application.Common.Dtos;
public sealed record OrderTrackingDto(string OrderNumber, string CurrentStatus, string? TrackingNumber, string? Carrier, DateTime? EstimatedDeliveryStart, DateTime? EstimatedDeliveryEnd, IEnumerable<OrderStatusHistoryDto> History);
