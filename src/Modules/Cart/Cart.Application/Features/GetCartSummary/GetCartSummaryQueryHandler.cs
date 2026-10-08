namespace Cart.Application.Features.GetCartSummary
{
    public sealed class GetCartSummaryQueryHandler(ICartDbContext dbContext, ICatalogPublicApi catalogApi, IPromotionsPublicApi promotionsApi) : IRequestHandler<GetCartSummaryQuery, CartSummaryDto>
    {
        public async Task<CartSummaryDto> Handle(GetCartSummaryQuery request, CancellationToken cancellationToken)
        {
            var cart = await dbContext.Carts
                .AsNoTracking()
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c =>
                    (request.UserId != null && c.UserId == request.UserId) ||
                    (request.SessionId != null && c.SessionId == request.SessionId), cancellationToken);

            if (cart == null) return new CartSummaryDto(0, 0, 0, 0);

            var variantIds = cart.Items.Where(i => !i.IsSavedForLater).Select(i => i.ProductVariantId).ToList();
            if (variantIds.Count == 0) return new CartSummaryDto(0, 0, 0, 0);

            var products = await catalogApi.GetBasketProductsAsync(variantIds, cancellationToken);
            var productDiscounts = (await promotionsApi.GetActiveDiscountsAsync(variantIds, cancellationToken))
                .ToDictionary(d => d.ProductVariantId);

            decimal subtotal = 0;
            foreach (var item in cart.Items.Where(i => !i.IsSavedForLater))
            {
                var p = products.FirstOrDefault(x => x.VariantId == item.ProductVariantId);
                if (p != null)
                {
                    var discount = productDiscounts.ContainsKey(item.ProductVariantId) ? productDiscounts[item.ProductVariantId].DiscountPercentage : 0m;
                    var finalPrice = Math.Round(Math.Max(0, p.Price - (p.Price * discount / 100m)), 2);
                    subtotal += finalPrice * item.Quantity;
                }
            }

            decimal discountValue = 0;
            if (!string.IsNullOrWhiteSpace(cart.AppliedCouponCode))
            {
                var coupon = await promotionsApi.GetCouponAsync(cart.AppliedCouponCode, cancellationToken);
                if (coupon != null && coupon.IsActive && coupon.ExpiresAt > DateTime.UtcNow && subtotal >= coupon.MinOrderAmount)
                {
                    if (coupon.DiscountType == "Percentage")
                        discountValue = Math.Round(subtotal * (coupon.DiscountValue / 100), 2);
                    else
                        discountValue = coupon.DiscountValue;
                }
            }

            decimal shipping = subtotal >= 50 ? 0 : 5.99m;
            decimal total = subtotal + shipping - discountValue;
            if (total < 0) total = 0;

            return new CartSummaryDto(subtotal, shipping, discountValue, total);
        }
    }
}