using Microsoft.EntityFrameworkCore;

namespace Cart.Application.Features.GetWishlists
{
    public sealed class GetWishlistsQueryHandler(ICartDbContext dbContext) : IRequestHandler<GetWishlistsQuery, IEnumerable<WishlistDto>>
    {
        public async Task<IEnumerable<WishlistDto>> Handle(GetWishlistsQuery request, CancellationToken cancellationToken)
        {
            var query = dbContext.Wishlists.AsNoTracking();

            if (request.UserId.HasValue && request.UserId.Value != Guid.Empty)
            {
                query = query.Where(w => w.UserId == request.UserId.Value);
            }
            else
            {
                return Enumerable.Empty<WishlistDto>();
            }

            return await query
                .Select(w => new WishlistDto(w.Id, w.Name, w.Items.Count))
                .ToListAsync(cancellationToken);
        }
    }
}