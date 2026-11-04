namespace Ordering.Domain.CheckoutAggregate
{
    internal sealed class CheckoutSession : AuditEntity
    {
        private CheckoutSession()
        {
        }
        public CheckoutSession(Guid? userId, string? sessionId)
        {
            SetUserId(userId);
            SetSessionId(sessionId);
            IsCompleted = false;
        }
        public Guid? UserId { get; private set; }
        public string SessionId { get; private set; } = null!;
        public string? Email { get; private set; }
        public string? FirstName { get; private set; }
        public string? LastName { get; private set; }
        public string? Address { get; private set; }
        public string? City { get; private set; }
        public string? ZipCode { get; private set; }
        public string? ShippingMethod { get; private set; }
        public decimal ShippingPrice { get; private set; }
        public string? PaymentMethod { get; private set; }
        public string? CardLast4 { get; private set; }
        public bool IsCompleted { get; private set; }
        public uint Version { get; private set; }
        public void SetShippingInfo(string email, string firstName, string lastName, string address, string city, string zipCode, string shippingMethod, decimal shippingPrice)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(email, "Email cannot be empty.");
            ArgumentException.ThrowIfNullOrWhiteSpace(address, "Address cannot be empty.");
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Address = address;
            City = city;
            ZipCode = zipCode;
            ShippingMethod = shippingMethod;
            ShippingPrice = shippingPrice;
        }
        public void SetPaymentInfo(string paymentMethod, string cardLast4)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(paymentMethod, "Payment method cannot be empty.");
            PaymentMethod = paymentMethod;
            CardLast4 = cardLast4;
        }
        public void MarkAsCompleted()
        {
            IsCompleted = true;
        }
        private void SetUserId(Guid? userId) => UserId = userId;
        private void SetSessionId(string? sessionId)
        {
            if (UserId.HasValue && UserId.Value != Guid.Empty)
            {
                SessionId = string.IsNullOrWhiteSpace(sessionId) ? Guid.NewGuid().ToString("N") : sessionId;
                return;
            }
            ArgumentException.ThrowIfNullOrWhiteSpace(sessionId, "Session ID cannot be empty.");
            SessionId = sessionId;
        }
    }
}