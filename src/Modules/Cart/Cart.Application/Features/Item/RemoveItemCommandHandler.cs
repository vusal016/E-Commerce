namespace Cart.Application.Features.Item;

public sealed class RemoveItemCommandHandler(ICartDbContext dbContext, IMediator mediator) : IRequestHandler<RemoveItemCommand, CartDto>
{
    public async Task<CartDto> Handle(RemoveItemCommand request, CancellationToken cancellationToken)
    {
        var cart = await dbContext.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c =>
                (request.UserId != null && c.UserId == request.UserId) || 
                (request.SessionId != null && c.SessionId == request.SessionId), cancellationToken);

        if (cart == null) throw new KeyNotFoundException("Cart not found.");

        var item = cart.Items.FirstOrDefault(i => i.Id == request.ItemId);
        if (item == null) throw new KeyNotFoundException("Cart item not found.");

        cart.Items.Remove(item);
        await dbContext.SaveChangesAsync(cancellationToken);

        return await mediator.Send(new GetCartQuery(request.UserId, request.SessionId), cancellationToken);
    }
}



