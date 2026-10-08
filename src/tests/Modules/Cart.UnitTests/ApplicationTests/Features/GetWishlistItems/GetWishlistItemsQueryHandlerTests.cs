namespace Cart.UnitTests.ApplicationTests.Features.GetWishlistItems;

public sealed class GetWishlistItemsQueryHandlerTests
{
    private readonly ICartDbContext _dbContext;
    private readonly ICatalogPublicApi _catalogApi;
    private readonly GetWishlistItemsQueryHandler _handler;

    public GetWishlistItemsQueryHandlerTests()
    {
        _dbContext = Substitute.For<ICartDbContext>();
        _catalogApi = Substitute.For<ICatalogPublicApi>();
        _handler = new GetWishlistItemsQueryHandler(_dbContext, _catalogApi);
    }

    [Fact]
    public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenUserIdIsNull()
    {
        var query = new GetWishlistItemsQuery(null, Guid.NewGuid());
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);
        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task Handle_ShouldThrowKeyNotFoundException_WhenWishlistDoesNotExist()
    {
        var userId = Guid.NewGuid();
        var query = new GetWishlistItemsQuery(userId, Guid.NewGuid());
        var mockWishlists = new List<Wishlist>().BuildMockDbSet();
        _dbContext.Wishlists.Returns(mockWishlists);

        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task Handle_ShouldCalculateDynamicStatusesCorrectly()
    {
        var userId = Guid.NewGuid();
        var wishlistId = Guid.NewGuid();
        var variantId = Guid.NewGuid();

        var wishlist = new Wishlist(userId, "Test");
        // Force Id via reflection since it's an entity
        typeof(Wishlist).GetProperty("Id")?.SetValue(wishlist, wishlistId);
        
        var item = new WishlistItem(wishlistId, variantId, 100m);
        wishlist.Items.Add(item);

        var mockWishlists = new List<Wishlist> { wishlist }.BuildMockDbSet();
        _dbContext.Wishlists.Returns(mockWishlists);

        // Price dropped from 100 to 80, stock is 0 -> priceDropped, outOfStock
        var catalogProduct = new BasketProductDto(variantId, "Product", "Red", "L", null, 80m, 100m, 0);
        _catalogApi.GetBasketProductsAsync(Arg.Any<IEnumerable<Guid>>(), Arg.Any<CancellationToken>())
            .Returns(new List<BasketProductDto> { catalogProduct });

        var query = new GetWishlistItemsQuery(userId, wishlistId);
        var result = await _handler.Handle(query, CancellationToken.None);

        var dtoList = result.ToList();
        dtoList.Should().HaveCount(1);
        dtoList[0].DynamicStatuses.Should().Contain("outOfStock");
        dtoList[0].DynamicStatuses.Should().Contain("priceDropped");
    }
}



