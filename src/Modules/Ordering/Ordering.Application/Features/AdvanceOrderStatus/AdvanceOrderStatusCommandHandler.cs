namespace Ordering.Application.Features.AdvanceOrderStatus
{
    internal sealed class AdvanceOrderStatusCommandHandler(IOrderingDbContext dbContext) : IRequestHandler<AdvanceOrderStatusCommand, bool>
    {
        public async Task<bool> Handle(AdvanceOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var order = await dbContext.Orders.Include(o => o.StatusHistory).FirstOrDefaultAsync(o => o.OrderNumber == request.OrderNumber && o.UserId == request.UserId, cancellationToken);
            if (order is null)
                throw new KeyNotFoundException("Order not found.");
            var flow = new[] { "Pending", "Processing", "Shipped", "Delivered" };
            var currentIndex = Array.IndexOf(flow, order.Status);
            if (currentIndex >= 0 && currentIndex < flow.Length - 1)
            {
                var nextStatus = flow[currentIndex + 1];
                order.ChangeStatus(nextStatus);
                await dbContext.SaveChangesAsync(cancellationToken);
                return true;
            }
            return false;
        }
    }
}