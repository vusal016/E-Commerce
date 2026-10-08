namespace Ordering.Application.Features.Reorder;

internal sealed class ReorderCommandHandler(IOrderingDbContext dbContext, ICartPublicApi cartApi) : IRequestHandler<ReorderCommand, ReorderResultDto>
{
    public async Task<ReorderResultDto> Handle(ReorderCommand request, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders.Include(o => o.Items).AsNoTracking().FirstOrDefaultAsync(o => o.Id == request.OrderId && (request.UserId == null || o.UserId == request.UserId), cancellationToken);

        if (order is null)
            throw new KeyNotFoundException("Order not found.");

        var warnings = new List<string>();

        foreach (var item in order.Items)
        {
            var success = await cartApi.AddItemAsync(request.UserId, request.SessionId, item.ProductVariantId, item.Quantity, cancellationToken);
            if (!success)
            {
                warnings.Add($"{item.ProductNameSnapshot} is out of stock or unavailable.");
            }
        }

        return new ReorderResultDto(true, warnings);
    }
}
