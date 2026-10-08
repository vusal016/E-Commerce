namespace Cart.UnitTests.ApplicationTests.Features.GetCartSummary;

public sealed class GetCartSummaryQueryHandlerTests
{
    private readonly ICartDbContext _dbContext;
    private readonly IPromotionsPublicApi _promotionsApi;
    private readonly ICatalogPublicApi _catalogApi;
    private readonly GetCartSummaryQueryHandler _handler;

    public GetCartSummaryQueryHandlerTests()
    {
        _dbContext = Substitute.For<ICartDbContext>();
        _promotionsApi = Substitute.For<IPromotionsPublicApi>();
        _catalogApi = Substitute.For<ICatalogPublicApi>();
        _handler = new GetCartSummaryQueryHandler(_dbContext, _catalogApi, _promotionsApi);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptySummary_WhenCartDoesNotExist()
    {
        var mockCarts = new List<Cart.Domain.CartAggregate.Cart>().BuildMockDbSet();
        _dbContext.Carts.Returns(mockCarts);
        var query = new GetCartSummaryQuery(Guid.NewGuid(), null);
        var result = await _handler.Handle(query, CancellationToken.None);
        result.Total.Should().Be(0);
    }
}


