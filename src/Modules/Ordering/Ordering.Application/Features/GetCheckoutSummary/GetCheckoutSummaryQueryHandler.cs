namespace Ordering.Application.Features.GetCheckoutSummary;

public sealed class GetCheckoutSummaryQueryHandler(IOrderingDbContext orderingDb, ICartPublicApi cartPublicApi) : IRequestHandler<GetCheckoutSummaryQuery, CheckoutSummaryDto>
{
    public async Task<CheckoutSummaryDto> Handle(GetCheckoutSummaryQuery request, CancellationToken cancellationToken)
    {
        var checkOutSession = await orderingDb.CheckoutSessions.AsNoTracking().FirstOrDefaultAsync(cs => (request.UserId != null && cs.UserId == request.UserId) || (request.SessionId != null && cs.SessionId == request.SessionId), cancellationToken);

        if (checkOutSession is null)
        {
            throw new KeyNotFoundException("Checkout session not found.");
        }

        var cartInfo = await cartPublicApi.GetCartForCheckoutAsync(request.UserId, request.SessionId, cancellationToken);
        
        if (cartInfo is null || cartInfo.Items.Count == 0)
        {
            throw new InvalidOperationException("Cart is empty.");
        }

        var items = cartInfo.Items.Select(i => new CheckoutSummaryItemDto(i.ProductVariantId, i.ProductName, i.Quantity, i.Price)).ToList();
        
        decimal totalPrice = cartInfo.Subtotal - cartInfo.CartDiscount + checkOutSession.ShippingPrice;
        if (totalPrice < 0)
        {
            totalPrice = 0;
        }

        return new CheckoutSummaryDto(checkOutSession.Address, checkOutSession.ShippingMethod, checkOutSession.PaymentMethod, checkOutSession.CardLast4, cartInfo.Subtotal, cartInfo.CartDiscount, checkOutSession.ShippingPrice, totalPrice, items);
    }
}
