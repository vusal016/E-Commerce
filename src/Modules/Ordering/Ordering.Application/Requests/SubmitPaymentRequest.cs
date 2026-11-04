namespace Ordering.Application.Requests
{
    internal sealed record SubmitPaymentRequest(string PaymentMethod, string? CardNumber, string? Cvv, string? ExpiryDate);
}