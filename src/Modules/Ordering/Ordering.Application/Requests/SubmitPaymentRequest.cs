namespace Ordering.Application.Requests;

public sealed record SubmitPaymentRequest(string PaymentMethod, string? CardNumber, string? Cvv, string? ExpiryDate);
