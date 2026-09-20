namespace Identity.Domain.PaymentMethod
{
    public sealed class PaymentMethod : BaseEntity
    {
        private PaymentMethod()
        {
        }

        public PaymentMethod(Guid userId, CardType cardType, string lastFourDigits, int expiryMonth, int expiryYear, string cardHolderName, bool isDefault)
        {
            SetUserId(userId);
            SetCardType(cardType);
            SetLastFourDigits(lastFourDigits);
            SetExpiryMonth(expiryMonth);
            SetExpiryYear(expiryYear);
            SetCardHolderName(cardHolderName);
            IsDefault = isDefault;
        }
        public Guid UserId { get; private set; }
        public CardType CardType { get; private set; }
        public string LastFourDigits { get; private set; }
        public int ExpiryMonth { get; private set; }
        public int ExpiryYear { get; private set; }
        public string CardHolderName { get; private set; }
        public bool IsDefault { get; private set; }


        private void SetUserId(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty.");
            UserId = userId;
        }

        private void SetCardType(CardType cardType)
        {
            if (!Enum.IsDefined(cardType))
                throw new ArgumentException("Invalid card type.");
            CardType = cardType;
        }
        private void SetLastFourDigits(string lastFourDigits)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(lastFourDigits, "Last four digits cannot be empty.");
            if (lastFourDigits.Length != 4 || !lastFourDigits.All(char.IsDigit))
                throw new ArgumentException("Last four digits must be exactly 4 digits.");
            LastFourDigits = lastFourDigits;
        }
        private void SetExpiryMonth(int expiryMonth)
        {
            if (expiryMonth < 1 || expiryMonth > 12)
                throw new ArgumentException("Expiry month must be between 1 and 12.");
            ExpiryMonth = expiryMonth;
        }
        private void SetExpiryYear(int expiryYear)
        {
            if (expiryYear < DateTime.UtcNow.Year)
                throw new ArgumentException("Expiry year cannot be in the past.");
            ExpiryYear = expiryYear;
        }
        private void SetCardHolderName(string cardHolderName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(cardHolderName, "Card holder name cannot be empty.");
            CardHolderName = cardHolderName;
        }
    }
}