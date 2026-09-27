namespace Cart.Application.Features.Queries;

public sealed record GetCartSummaryQuery(Guid? UserId, string? SessionId) : IRequest<CartSummaryDto>;



