
namespace Catalog.Application.EventHandlers;

internal sealed class OrderPlacedIntegrationEventHandler(ICatalogDbContext dbContext) : IIntegrationEventHandler<OrderPlacedIntegrationEvent>
{
    public async Task Handle(OrderPlacedIntegrationEvent integrationEvent, CancellationToken cancellationToken)
    {
        var variantIds = integrationEvent.Items.Select(i => i.ProductVariantId).ToList();

        var variants = await dbContext.ProductVariants.Where(v => variantIds.Contains(v.Id)).ToListAsync(cancellationToken);

        foreach (var item in integrationEvent.Items)
        {
            variants.FirstOrDefault(v => v.Id == item.ProductVariantId)?.DecreaseStock(item.Quantity);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

