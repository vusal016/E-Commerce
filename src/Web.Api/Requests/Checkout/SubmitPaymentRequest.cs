namespace Web.Api.Requests.Checkout
{
    internal sealed record SubmitPaymentRequest(string PaymentMethod, string? CardNumber, string? Cvv, string? ExpiryDate);
}