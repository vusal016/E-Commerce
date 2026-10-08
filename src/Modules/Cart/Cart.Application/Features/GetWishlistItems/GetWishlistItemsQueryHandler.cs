namespace Cart.Application.Features.GetWishlistItems
{
    public sealed class GetWishlistItemsQueryHandler(ICartDbContext dbContext, ICatalogPublicApi catalogApi) : IRequestHandler<GetWishlistItemsQuery, IEnumerable<WishlistItemDetailDto>>
    {
        public async Task<IEnumerable<WishlistItemDetailDto>> Handle(GetWishlistItemsQuery request, CancellationToken cancellationToken)
        {
            if (request.UserId is null || request.UserId == Guid.Empty)
                throw new UnauthorizedAccessException("Only authenticated users can view wishlists.");

            var wishlist = await dbContext.Wishlists
                .AsNoTracking()
                .Include(w => w.Items)
                .FirstOrDefaultAsync(w => w.Id == request.WishlistId && (w.UserId == request.UserId), cancellationToken);

            if (wishlist == null)
                throw new KeyNotFoundException("Wishlist not found.");

            if (wishlist.Items.Count == 0)
                return [];

            var variantIds = wishlist.Items.Select(i => i.ProductVariantId).ToList();
            var products = await catalogApi.GetBasketProductsAsync(variantIds, cancellationToken);

            var result = new List<WishlistItemDetailDto>();

            foreach (var item in wishlist.Items)
            {
                var product = products.FirstOrDefault(p => p.VariantId == item.ProductVariantId);
                if (product != null)
                {
                    var statuses = new List<string>();
                    if (product.StockQuantity <= 0)
                        statuses.Add("outOfStock");
                    else
                        statuses.Add("inStock");

                    if (product.Price < item.PriceAtAdd)
                        statuses.Add("priceDropped");

                    result.Add(new WishlistItemDetailDto(
                        item.Id,
                        item.ProductVariantId,
                        product.ProductName,
                        product.Color,
                        product.Size,
                        product.PrimaryImageUrl,
                        product.Price,
                        item.PriceAtAdd,
                        statuses
                    ));
                }
            }

            return result;
        }
    }

}