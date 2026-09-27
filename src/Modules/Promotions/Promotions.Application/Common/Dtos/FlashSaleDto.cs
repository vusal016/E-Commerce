namespace Promotions.Application.Common.Dtos;

public sealed record FlashSaleDto(
    Guid Id,
    string Name,
    DateTime StartsAt,
    DateTime EndsAt,
    bool IsActive,
    IReadOnlyList<FlashSaleItemDto> Items);
