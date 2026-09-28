namespace Cart.Application.Features.UpdateItemQuantity;

public sealed class UpdateItemQuantityCommandHandler(ICartDbContext dbContext, ICatalogPublicApi catalogPublicApi, IMediator mediator) : IRequestHandler<UpdateItemQuantityCommand, CartDto>
{
    public async Task<CartDto> Handle(UpdateItemQuantityCommand request, CancellationToken cancellationToken)
    {
        if (request.Quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.");

        var cart = await dbContext.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c =>
                (request.UserId != null && c.UserId == request.UserId) || 
                (request.SessionId != null && c.SessionId == request.SessionId), cancellationToken);

        if (cart == null) throw new KeyNotFoundException("Cart not found.");

        var item = cart.Items.FirstOrDefault(i => i.Id == request.ItemId);
        if (item == null) throw new KeyNotFoundException("Cart item not found.");

        var catalogProducts = await catalogPublicApi.GetBasketProductsAsync(new[] { item.ProductVariantId }, cancellationToken);
        var product = catalogProducts.FirstOrDefault();

        if (product == null) throw new KeyNotFoundException("Product variant not found.");

        if (request.Quantity > product.StockQuantity)
            throw new ArgumentException("Not enough stock available.");

        item.UpdateQuantity(request.Quantity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return await mediator.Send(new GetCartQuery(request.UserId, request.SessionId), cancellationToken);
    }
}




