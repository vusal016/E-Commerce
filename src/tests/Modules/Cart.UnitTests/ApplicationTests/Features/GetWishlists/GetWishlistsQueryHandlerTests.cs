namespace Cart.UnitTests.ApplicationTests.Features.GetWishlists;

public sealed class GetWishlistsQueryHandlerTests
{
    private readonly ICartDbContext _dbContext;
    private readonly GetWishlistsQueryHandler _handler;

    public GetWishlistsQueryHandlerTests()
    {
        _dbContext = Substitute.For<ICartDbContext>();
        _handler = new GetWishlistsQueryHandler(_dbContext);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenUserIdIsNull()
    {
        var query = new GetWishlistsQuery(null);
        var result = await _handler.Handle(query, CancellationToken.None);
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenUserIdIsEmpty()
    {
        var query = new GetWishlistsQuery(Guid.Empty);
        var result = await _handler.Handle(query, CancellationToken.None);
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldReturnWishlists_WhenUserHasWishlists()
    {
        var userId = Guid.NewGuid();
        var wishlist1 = new Wishlist(userId, "Electronics");
        var wishlist2 = new Wishlist(userId, "Books");
        
        var mockWishlists = new List<Wishlist> { wishlist1, wishlist2 }.BuildMockDbSet();
        _dbContext.Wishlists.Returns(mockWishlists);

        var query = new GetWishlistsQuery(userId);
        var result = await _handler.Handle(query, CancellationToken.None);

        var resultList = result.ToList();
        resultList.Should().HaveCount(2);
        resultList.Should().Contain(w => w.Name == "Electronics" && w.ItemCount == 0);
        resultList.Should().Contain(w => w.Name == "Books" && w.ItemCount == 0);
    }
}


