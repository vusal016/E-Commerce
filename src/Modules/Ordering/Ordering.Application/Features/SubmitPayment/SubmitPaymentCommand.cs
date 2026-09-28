namespace Ordering.Application.Features.SubmitPayment;

public sealed record SubmitPaymentCommand(Guid? UserId, string? SessionId, string PaymentMethod, string? CardNumber, string? Cvv, string? ExpiryDate) : IRequest<Guid>;
