namespace Cart.Application.Common.Dtos
{
    public sealed record WishlistDto(Guid Id, string Name, int ItemCount);
}