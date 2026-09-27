namespace Cart.Domain.WishlistAggregate
{
    public sealed class Wishlist : AuditEntity
    {
        private Wishlist()
        {
        }

        public Wishlist(Guid userId, string name = "All items")
        {
            SetUserId(userId);
            SetName(name);
        }

        public Guid UserId { get; private set; }
        public string Name { get; private set; } = null!;

        public ICollection<WishlistItem> Items { get; private set; } = [];

        private void SetUserId(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty.");
            UserId = userId;
        }

        private void SetName(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name, "Wishlist name cannot be empty.");
            Name = name;
        }
    }
}

