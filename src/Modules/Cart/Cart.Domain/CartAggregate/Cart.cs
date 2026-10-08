namespace Cart.Domain.CartAggregate
{
    public sealed class Cart : AuditEntity
    {
        private Cart()
        {
        }

        public Cart(Guid? userId, string sessionId)
        {
            SetUserId(userId);
            SetSessionId(sessionId);
        }

        public Guid? UserId { get; private set; }
        public string SessionId { get; private set; } = null!;

        public ICollection<CartItem> Items { get; private set; } = [];
        public string? AppliedCouponCode { get; private set; }

        public void ApplyCoupon(string couponCode)
        {
            AppliedCouponCode = couponCode;
        }

        public void RemoveCoupon()
        {
            AppliedCouponCode = null;
        }

        public void AssignUser(Guid userId)
        {
            UserId = userId;
        }

        private void SetUserId(Guid? userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty.");
            UserId = userId;
        }

        private void SetSessionId(string sessionId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(sessionId, "Session ID cannot be empty.");
            SessionId = sessionId;
        }
    }
}