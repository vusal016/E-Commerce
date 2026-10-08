namespace Cart.Application.Features.AddItem
{
    public sealed record AddItemCommand(Guid? UserId, string? SessionId, Guid ProductVariantId, int Quantity) : IRequest<CartDto>;
}