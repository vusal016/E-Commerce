namespace Cart.Application.Features.AddItem
{
    internal sealed class AddItemCommandHandler(ICartDbContext dbContext, ICatalogPublicApi catalogPublicApi, IMediator mediator) : IRequestHandler<AddItemCommand, CartDto>
    {
        public async Task<CartDto> Handle(AddItemCommand request, CancellationToken cancellationToken)
        {
            if (request.Quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");
            if (request.UserId == null && string.IsNullOrWhiteSpace(request.SessionId))
                throw new ArgumentException("Session ID is required for guest users.");
            var catalogProducts = await catalogPublicApi.GetBasketProductsAsync(new[] { request.ProductVariantId }, cancellationToken);
            var product = catalogProducts.FirstOrDefault();
            if (product == null)
                throw new KeyNotFoundException("Product variant not found.");
            var cart = await dbContext.Carts
                .Include(c => c.Items)
                .ForOwner(request.UserId, request.SessionId)
                .FirstOrDefaultAsync(cancellationToken);
            var existingItem = cart?.Items.FirstOrDefault(i => i.ProductVariantId == request.ProductVariantId);
            int newTotalQuantity = (existingItem?.Quantity ?? 0) + request.Quantity;
            if (newTotalQuantity > product.StockQuantity)
                throw new ArgumentException("Not enough stock available.");
            if (cart == null)
            {
                cart = new CartEntity(request.UserId, request.SessionId!);
                dbContext.Carts.Add(cart);
            }
            if (existingItem != null)
            {
                if(existingItem.IsSavedForLater)        
                    existingItem.ToggleSaveForLater();
                existingItem.UpdateQuantity(newTotalQuantity);
            }
            else
            {
                cart.Items.Add(new CartItem(cart.Id, request.ProductVariantId, request.Quantity, false));
            }
            await dbContext.SaveChangesAsync(cancellationToken);
            return await mediator.Send(new GetCartQuery(request.UserId, request.SessionId), cancellationToken);
        }
    }
}