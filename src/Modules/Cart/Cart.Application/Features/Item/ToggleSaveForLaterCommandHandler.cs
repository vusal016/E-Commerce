namespace Cart.Application.Features.Item;

public sealed class ToggleSaveForLaterCommandHandler(ICartDbContext dbContext, IMediator mediator) : IRequestHandler<ToggleSaveForLaterCommand, CartDto>
{
    public async Task<CartDto> Handle(ToggleSaveForLaterCommand request, CancellationToken cancellationToken)
    {
        var cart = await dbContext.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c =>
                (request.UserId != null && c.UserId == request.UserId) || 
                (request.SessionId != null && c.SessionId == request.SessionId), cancellationToken);

        if (cart == null) throw new KeyNotFoundException("Cart not found.");

        var item = cart.Items.FirstOrDefault(i => i.Id == request.ItemId);
        if (item == null) throw new KeyNotFoundException("Cart item not found.");

        item.ToggleSaveForLater();
        await dbContext.SaveChangesAsync(cancellationToken);

        return await mediator.Send(new GetCartQuery(request.UserId, request.SessionId), cancellationToken);
    }
}



