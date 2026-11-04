namespace Cart.Application.Features.GetWishlists
{
    internal sealed class GetWishlistsQueryHandler(ICartDbContext dbContext) : IRequestHandler<GetWishlistsQuery, IEnumerable<WishlistDto>>
    {
        public async Task<IEnumerable<WishlistDto>> Handle(GetWishlistsQuery request, CancellationToken cancellationToken)
        {
            if (!request.UserId.HasValue || request.UserId.Value == Guid.Empty)
            {
                return Enumerable.Empty<WishlistDto>();
            }
            return await dbContext.Wishlists
                .Where(w => w.UserId == request.UserId.Value)
                .Select(w => new WishlistDto(w.Id, w.Name, w.Items.Count))
                .ToListAsync(cancellationToken);
        }
    }
}