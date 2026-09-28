namespace Ordering.Application.Common.Interfaces
{
    public interface IOrderingDbContext
    {
        DbSet<Order> Orders { get; set; }
        DbSet<OrderItem> OrderItems { get; set; }
        DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }
        DbSet<ReturnRequest> ReturnRequests { get; set; }
        DbSet<CheckoutSession> CheckoutSessions { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
