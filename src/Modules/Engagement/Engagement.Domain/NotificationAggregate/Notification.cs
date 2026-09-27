namespace Engagement.Domain.NotificationAggregate
{
    public sealed class Notification : AuditEntity
    {
        private Notification()
        {
        }

        public Notification(Guid userId, string type, string title, string message, bool isRead)
        {
            SetUserId(userId);
            SetType(type);
            SetTitle(title);
            SetMessage(message);
            IsRead = isRead;
        }

        public Guid UserId { get; private set; }
        public string Type { get; private set; }
        public string Title { get; private set; }
        public string Message { get; private set; }
        public bool IsRead { get; private set; }

        private void SetUserId(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty.");
            UserId = userId;
        }

        private void SetType(string type)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(type, "Notification type cannot be empty.");
            Type = type;
        }

        private void SetTitle(string title)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(title, "Title cannot be empty.");
            Title = title;
        }

        private void SetMessage(string message)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(message, "Message cannot be empty.");
            Message = message;
        }
    }
}
