namespace Cart.UnitTests.ApplicationTests.Features.AddItem;

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
