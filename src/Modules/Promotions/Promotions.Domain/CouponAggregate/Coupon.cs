namespace Promotions.Domain.CouponAggregate
{
    public sealed class Coupon : AuditEntity
    {
        private Coupon()
        {
        }

        public Coupon(string code, string discountType, decimal discountValue, decimal minOrderAmount, DateTime expiresAt, bool isActive)
        {
            SetCode(code);
            SetDiscountType(discountType);
            SetDiscountValue(discountValue);
            SetMinOrderAmount(minOrderAmount);
            SetExpiresAt(expiresAt);
            IsActive = isActive;
        }

        public string Code { get; private set; }
        public string DiscountType { get; private set; }
        public decimal DiscountValue { get; private set; }
        public decimal MinOrderAmount { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public bool IsActive { get; private set; }

        private void SetCode(string code)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(code, "Coupon code cannot be empty.");
            Code = code;
        }

        private void SetDiscountType(string discountType)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(discountType, "Discount type cannot be empty.");
            DiscountType = discountType;
        }

        private void SetDiscountValue(decimal discountValue)
        {
            if (discountValue < 0)
                throw new ArgumentException("Discount value cannot be negative.");
            DiscountValue = discountValue;
        }

        private void SetMinOrderAmount(decimal minOrderAmount)
        {
            if (minOrderAmount < 0)
                throw new ArgumentException("Minimum order amount cannot be negative.");
            MinOrderAmount = minOrderAmount;
        }

        private void SetExpiresAt(DateTime expiresAt)
        {
            if (expiresAt == default)
                throw new ArgumentException("Expiry date must be provided.");
            ExpiresAt = expiresAt;
        }
    }
}
