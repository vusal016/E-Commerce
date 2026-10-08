namespace Cart.Application.Features.AddItem
{
    public sealed class AddItemCommandHandler(ICartDbContext dbContext, ICatalogPublicApi catalogPublicApi, IMediator mediator) : IRequestHandler<AddItemCommand, CartDto>
    {
        public async Task<CartDto> Handle(AddItemCommand request, CancellationToken cancellationToken)
        {
            if (request.Quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            var catalogProducts = await catalogPublicApi.GetBasketProductsAsync(new[] { request.ProductVariantId }, cancellationToken);
            var product = catalogProducts.FirstOrDefault();

            if (product == null)
                throw new KeyNotFoundException("Product variant not found.");

            var cart = await dbContext.Carts
        .Include(c => c.Items)
        .FirstOrDefaultAsync(c =>
            (request.UserId != null && c.UserId == request.UserId) ||
            (request.SessionId != null && c.SessionId == request.SessionId), cancellationToken);

            if (cart == null)
            {
                if (request.UserId == null && string.IsNullOrWhiteSpace(request.SessionId))
                    throw new ArgumentException("Session ID is required for guest users.");

                cart = new Domain.CartAggregate.Cart(request.UserId, request.SessionId!);
                dbContext.Carts.Add(cart);
            }

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductVariantId == request.ProductVariantId);
            int newTotalQuantity = existingItem != null ? existingItem.Quantity + request.Quantity : request.Quantity;

            if (newTotalQuantity > product.StockQuantity)
                throw new ArgumentException("Not enough stock available.");

            if (existingItem != null)
            {
                existingItem.UpdateQuantity(newTotalQuantity);
            }
            else
            {
                cart.Items.Add(new Domain.CartAggregate.CartItem(cart.Id, request.ProductVariantId, request.Quantity, false));
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            return await mediator.Send(new GetCartQuery(request.UserId, request.SessionId), cancellationToken);
        }
    }
}