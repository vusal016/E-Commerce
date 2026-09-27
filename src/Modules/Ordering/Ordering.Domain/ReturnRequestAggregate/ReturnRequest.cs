namespace Ordering.Domain.ReturnRequestAggregate
{
    public sealed class ReturnRequest : AuditEntity
    {
        private ReturnRequest()
        {
        }

        public ReturnRequest(Guid orderItemId, string reason, string resolutionType, string? exchangeSize, string? additionalNotes, string? photosJson, string status)
        {
            SetOrderItemId(orderItemId);
            SetDetails(reason, resolutionType, exchangeSize, additionalNotes, photosJson);
            SetStatus(status);
        }

        public Guid OrderItemId { get; private set; }
        public string Reason { get; private set; }
        public string ResolutionType { get; private set; }
        public string? ExchangeSize { get; private set; }
        public string? AdditionalNotes { get; private set; }
        public string? PhotosJson { get; private set; }
        public string Status { get; private set; }

        private void SetOrderItemId(Guid orderItemId)
        {
            if (orderItemId == Guid.Empty)
                throw new ArgumentException("Order item ID cannot be empty.");
            OrderItemId = orderItemId;
        }

        private void SetDetails(string reason, string resolutionType, string? exchangeSize, string? additionalNotes, string? photosJson)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(reason, "Reason cannot be empty.");
            ArgumentException.ThrowIfNullOrWhiteSpace(resolutionType, "Resolution type cannot be empty.");
            
            Reason = reason;
            ResolutionType = resolutionType;
            ExchangeSize = exchangeSize;
            AdditionalNotes = additionalNotes;
            PhotosJson = photosJson;
        }

        private void SetStatus(string status)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(status, "Status cannot be empty.");
            Status = status;
        }
    }
}
