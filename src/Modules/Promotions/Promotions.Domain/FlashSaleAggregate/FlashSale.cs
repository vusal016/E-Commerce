namespace Promotions.Domain.FlashSaleAggregate
{
    public sealed class FlashSale : AuditEntity
    {
        private FlashSale()
        {
        }

        public FlashSale(string name, DateTime startsAt, DateTime endsAt, bool isActive)
        {
            SetName(name);
            SetDates(startsAt, endsAt);
            IsActive = isActive;
        }

        public string Name { get; private set; }
        public DateTime StartsAt { get; private set; }
        public DateTime EndsAt { get; private set; }
        public bool IsActive { get; private set; }

        public ICollection<FlashSaleItem> Items { get; private set; } = [];

        private void SetName(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name, "Flash sale name cannot be empty.");
            Name = name;
        }

        private void SetDates(DateTime startsAt, DateTime endsAt)
        {
            if (startsAt == default)
                throw new ArgumentException("Starts at date must be provided.");
            if (endsAt == default)
                throw new ArgumentException("Ends at date must be provided.");
            if (startsAt >= endsAt)
                throw new ArgumentException("Starts at date must be before ends at date.");

            StartsAt = startsAt;
            EndsAt = endsAt;
        }
    }
}
