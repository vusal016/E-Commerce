namespace Ordering.Domain.OrderAggregate
{
    public sealed class Order : AuditEntity
    {
        private Order()
        {
        }

        public Order(Guid? userId, string orderNumber, string status, decimal subtotal, decimal shippingCost, decimal tax, decimal discount, decimal total, Guid shippingAddressId, Guid paymentMethodId, string shippingMethod, string? trackingNumber, string? carrier, DateTime placedAt, DateTime? estimatedDeliveryStart, DateTime? estimatedDeliveryEnd)
        {
            SetUserId(userId);
            SetOrderNumber(orderNumber);
            SetStatus(status);
            SetFinancials(subtotal, shippingCost, tax, discount, total);
            SetShippingAddressId(shippingAddressId);
            SetPaymentMethodId(paymentMethodId);
            SetShippingDetails(shippingMethod, trackingNumber, carrier);
            SetDates(placedAt, estimatedDeliveryStart, estimatedDeliveryEnd);
        }

        public Guid? UserId { get; private set; }
        public string OrderNumber { get; private set; }
        public string Status { get; private set; }
        public decimal Subtotal { get; private set; }
        public decimal ShippingCost { get; private set; }
        public decimal Tax { get; private set; }
        public decimal Discount { get; private set; }
        public decimal Total { get; private set; }
        public Guid ShippingAddressId { get; private set; }
        public Guid PaymentMethodId { get; private set; }
        public string ShippingMethod { get; private set; }
        public string? TrackingNumber { get; private set; }
        public string? Carrier { get; private set; }
        public DateTime PlacedAt { get; private set; }
        public DateTime? EstimatedDeliveryStart { get; private set; }
        public DateTime? EstimatedDeliveryEnd { get; private set; }

        public ICollection<OrderItem> Items { get; private set; } = [];
        public ICollection<OrderStatusHistory> StatusHistory { get; private set; } = [];

        public void AddItem(Guid productVariantId, string productName, decimal price, int quantity)
        {
            var item = new OrderItem(Id, productVariantId, productName, null, null, price, quantity);
            Items.Add(item);
        }

        private void SetUserId(Guid? userId) => UserId = userId;
        
        private void SetOrderNumber(string orderNumber)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(orderNumber, "Order number cannot be empty.");
            OrderNumber = orderNumber;
        }

        private void SetStatus(string status)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(status, "Status cannot be empty.");
            Status = status;
        }

        private void SetFinancials(decimal subtotal, decimal shippingCost, decimal tax, decimal discount, decimal total)
        {
            if (subtotal < 0)
                throw new ArgumentException("Subtotal cannot be negative.");
            if (shippingCost < 0)
                throw new ArgumentException("Shipping cost cannot be negative.");
            if (tax < 0)
                throw new ArgumentException("Tax cannot be negative.");
            if (discount < 0)
                throw new ArgumentException("Discount cannot be negative.");
            if (total < 0)
                throw new ArgumentException("Total cannot be negative.");
            
            Subtotal = subtotal;
            ShippingCost = shippingCost;
            Tax = tax;
            Discount = discount;
            Total = total;
        }

        private void SetShippingAddressId(Guid shippingAddressId)
        {
            if (shippingAddressId == Guid.Empty)
                throw new ArgumentException("Shipping address ID cannot be empty.");
            ShippingAddressId = shippingAddressId;
        }

        private void SetPaymentMethodId(Guid paymentMethodId)
        {
            if (paymentMethodId == Guid.Empty)
                throw new ArgumentException("Payment method ID cannot be empty.");
            PaymentMethodId = paymentMethodId;
        }

        private void SetShippingDetails(string shippingMethod, string? trackingNumber, string? carrier)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(shippingMethod, "Shipping method cannot be empty.");
            ShippingMethod = shippingMethod;
            TrackingNumber = trackingNumber;
            Carrier = carrier;
        }

        private void SetDates(DateTime placedAt, DateTime? estimatedDeliveryStart, DateTime? estimatedDeliveryEnd)
        {
            if (placedAt == default)
                throw new ArgumentException("Placed at date must be valid.");
            PlacedAt = placedAt;
            EstimatedDeliveryStart = estimatedDeliveryStart;
            EstimatedDeliveryEnd = estimatedDeliveryEnd;
        }
    }
}

