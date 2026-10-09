namespace Cart.Application.Features.ClearCart
{
    internal sealed class ClearCartCommandHandler(ICartDbContext dbContext) : IRequestHandler<ClearCartCommand>
    {
        public async Task Handle(ClearCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await dbContext.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => (request.UserId != null && c.UserId == request.UserId) || (request.SessionId != null && c.SessionId == request.SessionId), cancellationToken);

            if (cart != null)
            {
                dbContext.Carts.Remove(cart);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}