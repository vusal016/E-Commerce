namespace Ordering.Domain.OrderAggregate
{
    public sealed class OrderStatusHistory : AuditEntity
    {
        private OrderStatusHistory()
        {
        }

        public OrderStatusHistory(Guid orderId, string status)
        {
            SetOrderId(orderId);
            SetStatus(status);
            ChangedAt = DateTime.UtcNow;
        }

        public Guid OrderId { get; private set; }
        public string Status { get; private set; }
        public DateTime ChangedAt { get; private set; }

        public Order Order { get; private set; } = null!;

        private void SetOrderId(Guid orderId)
        {
            if (orderId == Guid.Empty)
                throw new ArgumentException("Order ID cannot be empty.");
            OrderId = orderId;
        }

        private void SetStatus(string status)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(status, "Status cannot be empty.");
            Status = status;
        }
    }
}
