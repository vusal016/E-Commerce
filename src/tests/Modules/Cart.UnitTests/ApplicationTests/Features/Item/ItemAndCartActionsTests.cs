
namespace Cart.UnitTests.ApplicationTests.Features.Item;

public sealed class AddItemCommandHandlerTests
{
    private readonly ICartDbContext _dbContext;
    private readonly ICatalogPublicApi _catalogPublicApi;
    private readonly IMediator _mediator;
    private readonly AddItemCommandHandler _handler;

    public AddItemCommandHandlerTests()
    {
        _dbContext = Substitute.For<ICartDbContext>();
        _catalogPublicApi = Substitute.For<ICatalogPublicApi>();
        _mediator = Substitute.For<IMediator>();
        _handler = new AddItemCommandHandler(_dbContext, _catalogPublicApi, _mediator);
    }

    [Fact]
    public async Task Handle_ShouldThrowArgumentException_WhenQuantityIsZero()
    {
        var command = new AddItemCommand(Guid.NewGuid(), null, Guid.NewGuid(), 0);
        await FluentActions.Invoking(() => _handler.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<ArgumentException>();
    }
}

public sealed class UpdateItemQuantityCommandHandlerTests
{
    private readonly ICartDbContext _dbContext;
    private readonly ICatalogPublicApi _catalogPublicApi;
    private readonly IMediator _mediator;
    private readonly UpdateItemQuantityCommandHandler _handler;

    public UpdateItemQuantityCommandHandlerTests()
    {
        _dbContext = Substitute.For<ICartDbContext>();
        _catalogPublicApi = Substitute.For<ICatalogPublicApi>();
        _mediator = Substitute.For<IMediator>();
        _handler = new UpdateItemQuantityCommandHandler(_dbContext, _catalogPublicApi, _mediator);
    }

    [Fact]
    public async Task Handle_ShouldThrowArgumentException_WhenQuantityIsZero()
    {
        var command = new UpdateItemQuantityCommand(Guid.NewGuid(), null, Guid.NewGuid(), 0);
        await FluentActions.Invoking(() => _handler.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<ArgumentException>();
    }
}

public sealed class RemoveItemCommandHandlerTests
{
    private readonly ICartDbContext _dbContext;
    private readonly IMediator _mediator;
    private readonly RemoveItemCommandHandler _handler;

    public RemoveItemCommandHandlerTests()
    {
        _dbContext = Substitute.For<ICartDbContext>();
        _mediator = Substitute.For<IMediator>();
        _handler = new RemoveItemCommandHandler(_dbContext, _mediator);
    }

    [Fact]
    public async Task Handle_ShouldThrowKeyNotFoundException_WhenCartDoesNotExist()
    {
        var mockCarts = new List<Cart.Domain.CartAggregate.Cart>().BuildMockDbSet();
        _dbContext.Carts.Returns(mockCarts);
        var command = new RemoveItemCommand(Guid.NewGuid(), null, Guid.NewGuid());
        await FluentActions.Invoking(() => _handler.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<KeyNotFoundException>();
    }
}

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





