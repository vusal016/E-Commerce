namespace Web.Api.Requests.Checkout;

public sealed record SubmitPaymentRequest(string PaymentMethod, string? CardNumber, string? Cvv, string? ExpiryDate);
