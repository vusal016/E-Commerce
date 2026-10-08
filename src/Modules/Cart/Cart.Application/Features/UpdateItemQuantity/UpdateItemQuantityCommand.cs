namespace Cart.Application.Features.UpdateItemQuantity
{
    public sealed record UpdateItemQuantityCommand(Guid? UserId, string? SessionId, Guid ItemId, int Quantity) : IRequest<CartDto>;
}