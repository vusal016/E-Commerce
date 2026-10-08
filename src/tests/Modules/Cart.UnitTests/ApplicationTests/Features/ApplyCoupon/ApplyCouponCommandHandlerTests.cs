namespace Cart.UnitTests.ApplicationTests.Features.ApplyCoupon;

public sealed class ApplyCouponCommandHandlerTests
{
    private readonly ICartDbContext _dbContext;
    private readonly IPromotionsPublicApi _promotionsApi;
    private readonly ICatalogPublicApi _catalogApi;
    private readonly IMediator _mediator;
    private readonly ApplyCouponCommandHandler _handler;

    public ApplyCouponCommandHandlerTests()
    {
        _dbContext = Substitute.For<ICartDbContext>();
        _promotionsApi = Substitute.For<IPromotionsPublicApi>();
        _catalogApi = Substitute.For<ICatalogPublicApi>();
        _mediator = Substitute.For<IMediator>();
        _handler = new ApplyCouponCommandHandler(_dbContext, _promotionsApi, _catalogApi, _mediator);
    }

    [Fact]
    public async Task Handle_ShouldThrowKeyNotFoundException_WhenCartDoesNotExist()
    {
        var mockCarts = new List<Cart.Domain.CartAggregate.Cart>().BuildMockDbSet();
        _dbContext.Carts.Returns(mockCarts);
        var command = new ApplyCouponCommand(Guid.NewGuid(), null, "SALE10");
        await FluentActions.Invoking(() => _handler.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<KeyNotFoundException>();
    }
}


