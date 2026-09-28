namespace Ordering.Application.Features.GetCheckoutSummary;

public sealed record GetCheckoutSummaryQuery(Guid? UserId, string? SessionId) : IRequest<CheckoutSummaryDto>;
