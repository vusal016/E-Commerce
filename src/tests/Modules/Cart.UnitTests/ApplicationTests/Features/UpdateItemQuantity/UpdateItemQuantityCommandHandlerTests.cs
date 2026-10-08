namespace Cart.UnitTests.ApplicationTests.Features.UpdateItemQuantity;

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


