namespace Cart.Application.Common.Dtos;

public sealed record CartDto(Guid Id, Guid? UserId, string? SessionId, IReadOnlyList<CartItemDto> Items);
