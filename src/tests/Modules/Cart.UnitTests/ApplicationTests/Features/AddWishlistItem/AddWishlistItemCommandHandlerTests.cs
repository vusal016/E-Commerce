namespace Cart.UnitTests.ApplicationTests.Features.AddWishlistItem;

public sealed class AddWishlistItemCommandHandlerTests
{
    private readonly ICartDbContext _dbContext;
    private readonly ICatalogPublicApi _catalogApi;
    private readonly AddWishlistItemCommandHandler _handler;

    public AddWishlistItemCommandHandlerTests()
    {
        _dbContext = Substitute.For<ICartDbContext>();
        _catalogApi = Substitute.For<ICatalogPublicApi>();
        _handler = new AddWishlistItemCommandHandler(_dbContext, _catalogApi);
    }

    [Fact]
    public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenUserIdIsNull()
    {
        var command = new AddWishlistItemCommand(null, Guid.NewGuid(), Guid.NewGuid());
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task Handle_ShouldThrowKeyNotFoundException_WhenWishlistDoesNotExist()
    {
        var userId = Guid.NewGuid();
        var command = new AddWishlistItemCommand(userId, Guid.NewGuid(), Guid.NewGuid());
        
        var mockWishlists = new List<Wishlist>().BuildMockDbSet();
        _dbContext.Wishlists.Returns(mockWishlists);

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Wishlist not found.");
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidOperationException_WhenItemAlreadyExists()
    {
        var userId = Guid.NewGuid();
        var wishlistId = Guid.NewGuid();
        var variantId = Guid.NewGuid();

        var wishlist = new Wishlist(userId, "Test");
        typeof(Wishlist).GetProperty("Id")?.SetValue(wishlist, wishlistId);
        wishlist.Items.Add(new WishlistItem(wishlistId, variantId, 10m));

        var mockWishlists = new List<Wishlist> { wishlist }.BuildMockDbSet();
        _dbContext.Wishlists.Returns(mockWishlists);

        var command = new AddWishlistItemCommand(userId, wishlistId, variantId);

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Item already exists in wishlist.");
    }

    [Fact]
    public async Task Handle_ShouldThrowKeyNotFoundException_WhenProductDoesNotExist()
    {
        var userId = Guid.NewGuid();
        var wishlistId = Guid.NewGuid();
        var variantId = Guid.NewGuid();

        var wishlist = new Wishlist(userId, "Test");
        typeof(Wishlist).GetProperty("Id")?.SetValue(wishlist, wishlistId);

        var mockWishlists = new List<Wishlist> { wishlist }.BuildMockDbSet();
        _dbContext.Wishlists.Returns(mockWishlists);

        _catalogApi.GetBasketProductsAsync(Arg.Any<IEnumerable<Guid>>(), Arg.Any<CancellationToken>())
            .Returns(new List<BasketProductDto>());

        var command = new AddWishlistItemCommand(userId, wishlistId, variantId);

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Product variant not found.");
    }

    [Fact]
    public async Task Handle_ShouldAddItemAndReturnId_WhenValidRequest()
    {
        var userId = Guid.NewGuid();
        var wishlistId = Guid.NewGuid();
        var variantId = Guid.NewGuid();

        var wishlist = new Wishlist(userId, "Test");
        typeof(Wishlist).GetProperty("Id")?.SetValue(wishlist, wishlistId);

        var mockWishlists = new List<Wishlist> { wishlist }.BuildMockDbSet();
        _dbContext.Wishlists.Returns(mockWishlists);

        var product = new BasketProductDto(variantId, "Prod", "Red", "L", null, 150m, 150m, 10);
        _catalogApi.GetBasketProductsAsync(Arg.Any<IEnumerable<Guid>>(), Arg.Any<CancellationToken>())
            .Returns(new List<BasketProductDto> { product });

        var command = new AddWishlistItemCommand(userId, wishlistId, variantId);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeEmpty();
        wishlist.Items.Should().HaveCount(1);
        wishlist.Items.First().ProductVariantId.Should().Be(variantId);
        wishlist.Items.First().PriceAtAdd.Should().Be(150m);
        await _dbContext.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}


