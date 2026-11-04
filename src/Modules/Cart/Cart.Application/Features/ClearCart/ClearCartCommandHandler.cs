namespace Cart.Application.Features.ClearCart
{
    internal sealed class ClearCartCommandHandler(ICartDbContext dbContext) : IRequestHandler<ClearCartCommand>
    {
        public async Task Handle(ClearCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await dbContext.Carts.Include(c => c.Items).ForOwner(request.UserId, request.SessionId).FirstOrDefaultAsync(cancellationToken);
            if (cart != null)
            {
                dbContext.Carts.Remove(cart);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}