namespace Identity.Domain.RefreshToken
{
    public sealed class RefreshToken:AuditEntity
    {
        private RefreshToken()
        {
            
        }
        public RefreshToken(Guid userId, string token, DateTime expiresAt)
        {
            SetUserId(userId);
            SetToken(token);
            ExpiresAt = expiresAt;
        }

        public Guid UserId { get;private set; }
        public string Token { get;private set; }
        public DateTime ExpiresAt { get;private set; }

        private void SetUserId(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty.");
            UserId = userId;
        }
        private void SetToken(string token)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(token, "Token cannot be empty.");
            Token = token;
        }
    }
}