namespace Cart.UnitTests.ApplicationTests.Features.NotifyWishlistItem;

public sealed class NotifyWishlistItemCommandHandlerTests
{
    private readonly ICartDbContext _dbContext;
    private readonly ICatalogPublicApi _catalogApi;
    private readonly NotifyWishlistItemCommandHandler _handler;

    public NotifyWishlistItemCommandHandlerTests()
    {
        _dbContext = Substitute.For<ICartDbContext>();
        _catalogApi = Substitute.For<ICatalogPublicApi>();
        _handler = new NotifyWishlistItemCommandHandler(_dbContext, _catalogApi);
    }

    [Fact]
    public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenUserIdIsNull()
    {
        var command = new NotifyWishlistItemCommand(null, Guid.NewGuid(), Guid.NewGuid());
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task Handle_ShouldThrowKeyNotFoundException_WhenWishlistDoesNotExist()
    {
        var userId = Guid.NewGuid();
        var command = new NotifyWishlistItemCommand(userId, Guid.NewGuid(), Guid.NewGuid());
        
        var mockWishlists = new List<Wishlist>().BuildMockDbSet();
        _dbContext.Wishlists.Returns(mockWishlists);

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Wishlist not found.");
    }

    [Fact]
    public async Task Handle_ShouldThrowKeyNotFoundException_WhenItemDoesNotExist()
    {
        var userId = Guid.NewGuid();
        var wishlistId = Guid.NewGuid();
        var itemId = Guid.NewGuid();

        var wishlist = new Wishlist(userId, "Test");
        typeof(Wishlist).GetProperty("Id")?.SetValue(wishlist, wishlistId);

        var mockWishlists = new List<Wishlist> { wishlist }.BuildMockDbSet();
        _dbContext.Wishlists.Returns(mockWishlists);

        var command = new NotifyWishlistItemCommand(userId, wishlistId, itemId);

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Item not found in wishlist.");
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidOperationException_WhenProductIsInStock()
    {
        var userId = Guid.NewGuid();
        var wishlistId = Guid.NewGuid();
        var itemId = Guid.NewGuid();
        var variantId = Guid.NewGuid();

        var wishlist = new Wishlist(userId, "Test");
        typeof(Wishlist).GetProperty("Id")?.SetValue(wishlist, wishlistId);

        var item = new WishlistItem(wishlistId, variantId, 150m);
        typeof(WishlistItem).GetProperty("Id")?.SetValue(item, itemId);
        wishlist.Items.Add(item);

        var mockWishlists = new List<Wishlist> { wishlist }.BuildMockDbSet();
        _dbContext.Wishlists.Returns(mockWishlists);

        var product = new BasketProductDto(variantId, "Prod", "Red", "L", null, 150m, 150m, 10);
        _catalogApi.GetBasketProductsAsync(Arg.Any<IEnumerable<Guid>>(), Arg.Any<CancellationToken>())
            .Returns(new List<BasketProductDto> { product });

        var command = new NotifyWishlistItemCommand(userId, wishlistId, itemId);

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Product is already in stock.");
    }

    [Fact]
    public async Task Handle_ShouldEnableNotification_WhenValidRequestAndOutOfStock()
    {
        var userId = Guid.NewGuid();
        var wishlistId = Guid.NewGuid();
        var itemId = Guid.NewGuid();
        var variantId = Guid.NewGuid();

        var wishlist = new Wishlist(userId, "Test");
        typeof(Wishlist).GetProperty("Id")?.SetValue(wishlist, wishlistId);

        var item = new WishlistItem(wishlistId, variantId, 150m);
        typeof(WishlistItem).GetProperty("Id")?.SetValue(item, itemId);
        wishlist.Items.Add(item);

        var mockWishlists = new List<Wishlist> { wishlist }.BuildMockDbSet();
        _dbContext.Wishlists.Returns(mockWishlists);

        var product = new BasketProductDto(variantId, "Prod", "Red", "L", null, 150m, 150m, 0);
        _catalogApi.GetBasketProductsAsync(Arg.Any<IEnumerable<Guid>>(), Arg.Any<CancellationToken>())
            .Returns(new List<BasketProductDto> { product });

        var command = new NotifyWishlistItemCommand(userId, wishlistId, itemId);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeTrue();
        item.WantsRestockNotification.Should().BeTrue();
        await _dbContext.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}


