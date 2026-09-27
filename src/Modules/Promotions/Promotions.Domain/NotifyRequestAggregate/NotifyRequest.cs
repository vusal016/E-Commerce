namespace Promotions.Domain.NotifyRequestAggregate
{
    public sealed class NotifyRequest : AuditEntity
    {
        private NotifyRequest()
        {
        }

        public NotifyRequest(Guid flashSaleId, string email)
        {
            SetFlashSaleId(flashSaleId);
            SetEmail(email);
        }

        public Guid FlashSaleId { get; private set; }
        public string Email { get; private set; }

        private void SetFlashSaleId(Guid flashSaleId)
        {
            if (flashSaleId == Guid.Empty)
                throw new ArgumentException("Flash sale ID cannot be empty.");
            FlashSaleId = flashSaleId;
        }

        private void SetEmail(string email)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(email, "Email cannot be empty.");
            Email = email;
        }
    }
}
