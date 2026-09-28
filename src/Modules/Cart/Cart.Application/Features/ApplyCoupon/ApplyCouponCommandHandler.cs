namespace Cart.Application.Features.ApplyCoupon;

public sealed class ApplyCouponCommandHandler(ICartDbContext dbContext, IPromotionsPublicApi promotionsApi, ICatalogPublicApi catalogApi, IMediator mediator) : IRequestHandler<ApplyCouponCommand, CartDto>
{
    public async Task<CartDto> Handle(ApplyCouponCommand request, CancellationToken cancellationToken)
    {
        var cart = await dbContext.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c =>
                (request.UserId != null && c.UserId == request.UserId) || 
                (request.SessionId != null && c.SessionId == request.SessionId), cancellationToken);

        if (cart == null) throw new KeyNotFoundException("Cart not found.");

        var coupon = await promotionsApi.GetCouponAsync(request.Code, cancellationToken);
        if (coupon == null || !coupon.IsActive || coupon.ExpiresAt < DateTime.UtcNow)
            throw new ArgumentException("Invalid or expired coupon.");

        var variantIds = cart.Items.Where(i => !i.IsSavedForLater).Select(i => i.ProductVariantId).ToList();
        var products = await catalogApi.GetBasketProductsAsync(variantIds, cancellationToken);
        
        decimal subtotal = 0;
        foreach (var item in cart.Items.Where(i => !i.IsSavedForLater))
        {
            var p = products.FirstOrDefault(x => x.VariantId == item.ProductVariantId);
            if (p != null) subtotal += p.Price * item.Quantity;
        }

        if (subtotal < coupon.MinOrderAmount)
            throw new ArgumentException($"Minimum order amount of {coupon.MinOrderAmount} is required.");

        cart.ApplyCoupon(request.Code);
        await dbContext.SaveChangesAsync(cancellationToken);

        return await mediator.Send(new GetCartQuery(request.UserId, request.SessionId), cancellationToken);
    }
}




