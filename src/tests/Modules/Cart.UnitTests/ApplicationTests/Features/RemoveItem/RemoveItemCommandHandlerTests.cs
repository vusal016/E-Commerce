namespace Cart.UnitTests.ApplicationTests.Features.RemoveItem;

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
