namespace Cart.UnitTests.ApplicationTests.Features.AddItem
{
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
        [Fact]
        public async Task Handle_WhenUserHasNoCartAndItemOutOfStock_ShouldThrowAndNotAddCart()
        {
            Guid userId = Guid.NewGuid();
            Guid outOfStockVariantId = Guid.NewGuid();
            DbSet<CartEntity> emptyCarts = new List<CartEntity>().BuildMockDbSet();
            _dbContext.Carts.Returns(emptyCarts);
            _catalogPublicApi.GetBasketProductsAsync(
                Arg.Is<IEnumerable<Guid>>(ids => ids.Contains(outOfStockVariantId)),
                Arg.Any<CancellationToken>())
                .Returns(new List<BasketProductDto>
                {
                    new BasketProductDto(outOfStockVariantId, "Out of Stock Product", "Black", "M", null, 50m, 50m, 0)
                });
            var command = new AddItemCommand(userId, null, outOfStockVariantId, 1);
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Not enough stock available.");
            _dbContext.Carts.DidNotReceive().Add(Arg.Any<CartEntity>());
        }
    }
}