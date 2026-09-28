    namespace Cart.Application.Features.GetCart;

public sealed class GetCartQueryHandler(ICartDbContext dbContext, ICatalogPublicApi catalogPublicApi, IPromotionsPublicApi promotionsApi) : IRequestHandler<GetCartQuery, CartDto>
{
    public async Task<CartDto> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var cart = await dbContext.Carts
            .AsNoTracking()
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c =>
                (request.UserId != null && c.UserId == request.UserId) || c.SessionId == request.SessionId, cancellationToken);

        if (cart is null) throw new KeyNotFoundException("Basket not found.");

        var activeItems = cart.Items.Where(i => !i.IsSavedForLater).ToList();
        var variantIds = activeItems.Select(i => i.ProductVariantId).ToList();

        var productBasket = await catalogPublicApi.GetBasketProductsAsync(variantIds, cancellationToken);
        var discounts = (await promotionsApi.GetActiveDiscountsAsync(variantIds, cancellationToken))
            .ToDictionary(d => d.ProductVariantId);

        var itemDto = activeItems.Select(item =>
        {
            var basket = productBasket.FirstOrDefault(p => p.VariantId == item.ProductVariantId);
            var stockWarning = basket != null && basket.StockQuantity <= 5 && basket.StockQuantity > 0
                ? "Only " + basket.StockQuantity + " left"
                : null;

            var discount = discounts.ContainsKey(item.ProductVariantId) ? discounts[item.ProductVariantId].DiscountPercentage : 0m;
            var basePrice = basket?.Price ?? 0;
            var finalPrice = Math.Round(Math.Max(0, basePrice - (basePrice * discount / 100m)), 2);

            return new CartItemDto(
                item.Id,
                item.ProductVariantId,
                item.Quantity,
                basket?.ProductName ?? "Unknown",
                finalPrice,
                basket?.PrimaryImageUrl,
                stockWarning);
        }).ToList();

        var dto = new CartDto(
            cart.Id,
            cart.UserId,
            cart.SessionId,
            itemDto
        );
        return dto;
    }
}
