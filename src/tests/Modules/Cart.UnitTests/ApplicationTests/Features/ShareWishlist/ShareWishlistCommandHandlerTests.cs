namespace Cart.UnitTests.ApplicationTests.Features.ShareWishlist;

public sealed class ShareWishlistCommandHandlerTests
{
    private readonly ICartDbContext _dbContext;
    private readonly ShareWishlistCommandHandler _handler;

    public ShareWishlistCommandHandlerTests()
    {
        _dbContext = Substitute.For<ICartDbContext>();
        _handler = new ShareWishlistCommandHandler(_dbContext);
    }

    [Fact]
    public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenUserIdIsNull()
    {
        var command = new ShareWishlistCommand(null, Guid.NewGuid(), "http://localhost");
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task Handle_ShouldThrowKeyNotFoundException_WhenWishlistDoesNotExist()
    {
        var userId = Guid.NewGuid();
        var command = new ShareWishlistCommand(userId, Guid.NewGuid(), "http://localhost");
        
        var mockWishlists = new List<Wishlist>().BuildMockDbSet();
        _dbContext.Wishlists.Returns(mockWishlists);

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Wishlist not found.");
    }

    [Fact]
    public async Task Handle_ShouldGenerateShareTokenAndReturnUrl_WhenValidRequest()
    {
        var userId = Guid.NewGuid();
        var wishlistId = Guid.NewGuid();

        var wishlist = new Wishlist(userId, "Test");
        typeof(Wishlist).GetProperty("Id")?.SetValue(wishlist, wishlistId);

        var mockWishlists = new List<Wishlist> { wishlist }.BuildMockDbSet();
        _dbContext.Wishlists.Returns(mockWishlists);

        var command = new ShareWishlistCommand(userId, wishlistId, "https://api.example.com");

        var result = await _handler.Handle(command, CancellationToken.None);

        wishlist.ShareToken.Should().NotBeNull();
        result.Should().Be($"https://api.example.com/shared-wishlists/{wishlist.ShareToken}");
        await _dbContext.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldNotChangeShareToken_WhenAlreadyGenerated()
    {
        var userId = Guid.NewGuid();
        var wishlistId = Guid.NewGuid();

        var wishlist = new Wishlist(userId, "Test");
        typeof(Wishlist).GetProperty("Id")?.SetValue(wishlist, wishlistId);
        wishlist.GenerateShareToken();
        var originalToken = wishlist.ShareToken;

        var mockWishlists = new List<Wishlist> { wishlist }.BuildMockDbSet();
        _dbContext.Wishlists.Returns(mockWishlists);

        var command = new ShareWishlistCommand(userId, wishlistId, "https://api.example.com/");

        var result = await _handler.Handle(command, CancellationToken.None);

        wishlist.ShareToken.Should().Be(originalToken);
        result.Should().Be($"https://api.example.com/shared-wishlists/{originalToken}");
        await _dbContext.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}


