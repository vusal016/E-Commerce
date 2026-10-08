namespace Cart.UnitTests.ApplicationTests.Features.RemoveWishlistItem;

public sealed class RemoveWishlistItemCommandHandlerTests
{
    private readonly ICartDbContext _dbContext;
    private readonly RemoveWishlistItemCommandHandler _handler;

    public RemoveWishlistItemCommandHandlerTests()
    {
        _dbContext = Substitute.For<ICartDbContext>();
        _handler = new RemoveWishlistItemCommandHandler(_dbContext);
    }

    [Fact]
    public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenUserIdIsNull()
    {
        var command = new RemoveWishlistItemCommand(null, Guid.NewGuid(), Guid.NewGuid());
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task Handle_ShouldThrowKeyNotFoundException_WhenWishlistDoesNotExist()
    {
        var userId = Guid.NewGuid();
        var command = new RemoveWishlistItemCommand(userId, Guid.NewGuid(), Guid.NewGuid());
        
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

        var command = new RemoveWishlistItemCommand(userId, wishlistId, itemId);

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Item not found in wishlist.");
    }

    [Fact]
    public async Task Handle_ShouldRemoveItem_WhenValidRequest()
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

        var command = new RemoveWishlistItemCommand(userId, wishlistId, itemId);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeTrue();
        wishlist.Items.Should().BeEmpty();
        await _dbContext.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}


