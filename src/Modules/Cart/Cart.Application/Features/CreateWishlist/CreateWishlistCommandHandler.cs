namespace Cart.Application.Features.CreateWishlist
{
    public sealed class CreateWishlistCommandHandler(ICartDbContext dbContext) : IRequestHandler<CreateWishlistCommand, Guid>
    {
        public async Task<Guid> Handle(CreateWishlistCommand request, CancellationToken cancellationToken)
        {
            if (request.UserId is null || request.UserId == Guid.Empty)
                throw new UnauthorizedAccessException("Only authenticated users can create wishlists.");

            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("Wishlist name cannot be empty.");

            var wishlist = new Wishlist(request.UserId.Value, request.Name);

            dbContext.Wishlists.Add(wishlist);
            await dbContext.SaveChangesAsync(cancellationToken);

            return wishlist.Id;
        }
    }
}