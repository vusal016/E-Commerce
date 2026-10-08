namespace Cart.UnitTests.ApplicationTests.Features.CreateWishlist;

public sealed class CreateWishlistCommandHandlerTests
{
    private readonly ICartDbContext _dbContext;
    private readonly CreateWishlistCommandHandler _handler;

    public CreateWishlistCommandHandlerTests()
    {
        _dbContext = Substitute.For<ICartDbContext>();
        _handler = new CreateWishlistCommandHandler(_dbContext);
    }

    [Fact]
    public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenUserIdIsNull()
    {
        var command = new CreateWishlistCommand(null, "My Wishlist");
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task Handle_ShouldThrowArgumentException_WhenNameIsEmpty()
    {
        var command = new CreateWishlistCommand(Guid.NewGuid(), " ");
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Wishlist name cannot be empty.");
    }

    [Fact]
    public async Task Handle_ShouldCreateWishlistAndReturnId_WhenValidRequest()
    {
        var userId = Guid.NewGuid();
        var command = new CreateWishlistCommand(userId, "Favorites");

        var mockWishlists = new List<Wishlist>().BuildMockDbSet();
        _dbContext.Wishlists.Returns(mockWishlists);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeEmpty();
        _dbContext.Wishlists.Received(1).Add(Arg.Is<Wishlist>(w => w.UserId == userId && w.Name == "Favorites"));
        await _dbContext.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}


