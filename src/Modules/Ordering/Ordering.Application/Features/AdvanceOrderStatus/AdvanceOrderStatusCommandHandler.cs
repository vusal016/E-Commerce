using Microsoft.EntityFrameworkCore;
using Ordering.Application.Common.Interfaces;

namespace Ordering.Application.Features.AdvanceOrderStatus;

internal sealed class AdvanceOrderStatusCommandHandler(IOrderingDbContext dbContext) : MediatR.IRequestHandler<AdvanceOrderStatusCommand, bool>
{
    public async Task<bool> Handle(AdvanceOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders.Include(o => o.StatusHistory).FirstOrDefaultAsync(o => o.OrderNumber == request.OrderNumber && (request.UserId == null || o.UserId == request.UserId), cancellationToken);

        if (order is null)
            throw new KeyNotFoundException("Order not found.");

        var flow = new[] { "Pending", "Processing", "Shipped", "Delivered" };
        var currentIndex = Array.IndexOf(flow, order.Status);

        if (currentIndex >= 0 && currentIndex < flow.Length - 1)
        {
            var nextStatus = flow[currentIndex + 1];
            order.ChangeStatus(nextStatus);
            
            // Fix: Explicitly mark the newly added history item as Added
            // because EF Core assumes non-empty Guids in tracked collections are Modified.

            await dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        return false;
    }
}

