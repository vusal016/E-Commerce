namespace Cart.Application.Features.MergeCart;

public sealed class MergeCartCommandHandler(ICartDbContext dbContext, IMediator mediator) : IRequestHandler<MergeCartCommand, CartDto>
{
    public async Task<CartDto> Handle(MergeCartCommand request, CancellationToken cancellationToken)
    {
        var userCart = await dbContext.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == request.UserId, cancellationToken);

        var guestCart = await dbContext.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.SessionId == request.SessionId && c.UserId == null, cancellationToken);

        if (guestCart == null)
            return await mediator.Send(new GetCartQuery(request.UserId, null), cancellationToken);

        if (userCart == null)
        {
            guestCart.AssignUser(request.UserId);
        }
        else
        {
            foreach (var guestItem in guestCart.Items)
            {
                var existing = userCart.Items.FirstOrDefault(i => i.ProductVariantId == guestItem.ProductVariantId);
                if (existing != null)
                {
                    existing.UpdateQuantity(existing.Quantity + guestItem.Quantity);
                }
                else
                {
                    userCart.Items.Add(new Domain.CartAggregate.CartItem(userCart.Id, guestItem.ProductVariantId, guestItem.Quantity, guestItem.IsSavedForLater));
                }
            }
            if (guestCart.AppliedCouponCode != null && userCart.AppliedCouponCode == null)
            {
                userCart.ApplyCoupon(guestCart.AppliedCouponCode);
            }
            dbContext.Carts.Remove(guestCart);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return await mediator.Send(new GetCartQuery(request.UserId, null), cancellationToken);
    }
}




