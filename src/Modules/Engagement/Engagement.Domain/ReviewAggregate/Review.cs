namespace Engagement.Domain.ReviewAggregate
{
    public sealed class Review : AuditEntity
    {
        private Review()
        {
        }
        public Review(Guid productId, Guid userId, Guid orderItemId, decimal rating, string? title, string? reviewText, string? fitRating, string? photosJson)
        {
            SetIds(productId, userId, orderItemId);
            SetRating(rating);
            Title = title;
            ReviewText = reviewText;
            FitRating = fitRating;
            PhotosJson = photosJson;
        }
        public Guid ProductId { get; private set; }
        public Guid UserId { get; private set; }
        public Guid OrderItemId { get; private set; }
        public decimal Rating { get; private set; }
        public string? Title { get; private set; }
        public string? ReviewText { get; private set; }
        public string? FitRating { get; private set; }
        public string? PhotosJson { get; private set; }
        private void SetIds(Guid productId, Guid userId, Guid orderItemId)
        {
            if (productId == Guid.Empty)
                throw new ArgumentException("Product ID cannot be empty.");
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty.");
            if (orderItemId == Guid.Empty)
                throw new ArgumentException("Order item ID cannot be empty.");
            ProductId = productId;
            UserId = userId;
            OrderItemId = orderItemId;
        }
        private void SetRating(decimal rating)
        {
            if (rating < 0 || rating > 5)
                throw new ArgumentException("Rating must be between 0 and 5.");
            Rating = rating;
        }
    }
}