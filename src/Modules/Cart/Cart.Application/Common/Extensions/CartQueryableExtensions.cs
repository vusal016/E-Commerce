namespace Cart.Application.Common.Extensions
{
    internal static class CartQueryableExtensions
    {
        public static IQueryable<CartEntity> ForOwner(this IQueryable<CartEntity> query, Guid? userId, string? sessionId)
        {
            if (userId.HasValue && userId.Value != Guid.Empty)
            {
                return query.Where(c => c.UserId == userId.Value);
            }
            if (!string.IsNullOrWhiteSpace(sessionId))
            {
                return query.Where(c => c.UserId == null && c.SessionId == sessionId);
            }
            return query.Where(_ => false);
        }
    }
}