namespace Ordering.Application.Common.Dtos;
public sealed record OrderStatusHistoryDto(Guid Id, string Status, DateTime ChangedAt);
