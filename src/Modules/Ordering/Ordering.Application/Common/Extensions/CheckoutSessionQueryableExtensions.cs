namespace Ordering.Application.Common.Extensions
{
    internal static class CheckoutSessionQueryableExtensions
    {
        public static IQueryable<CheckoutSession> ForOwner(this IQueryable<CheckoutSession> query, Guid? userId, string? sessionId)
        {
            if (userId.HasValue && userId.Value != Guid.Empty)
            {
                return query.Where(cs => cs.UserId == userId.Value && !cs.IsCompleted);
            }
            if (!string.IsNullOrWhiteSpace(sessionId))
            {
                return query.Where(cs => cs.UserId == null && cs.SessionId == sessionId && !cs.IsCompleted);
            }
            return query.Where(_ => false);
        }
    }
}