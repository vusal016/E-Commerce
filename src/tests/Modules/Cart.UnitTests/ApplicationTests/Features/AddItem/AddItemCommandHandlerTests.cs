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
        public async Task Handle_WhenUserHasNoCartAndFirstItemOutOfStockAndSecondInStock_ShouldOnlyAddOneCart()
        {
            Guid userId = Guid.NewGuid();
            Guid outOfStockVariantId = Guid.NewGuid();
            Guid inStockVariantId = Guid.NewGuid();
            Microsoft.EntityFrameworkCore.DbSet<CartEntity> emptyCarts = new List<CartEntity>().BuildMockDbSet();
            _dbContext.Carts.Returns(emptyCarts);
            _catalogPublicApi.GetBasketProductsAsync(
                Arg.Is<IEnumerable<Guid>>(ids => ids.Contains(outOfStockVariantId)),
                Arg.Any<CancellationToken>())
                .Returns(new List<BasketProductDto>
                {
                    new BasketProductDto(outOfStockVariantId, "Out of Stock Product", "Black", "M", null, 50m, 50m, 0)
                });
            _catalogPublicApi.GetBasketProductsAsync(
                Arg.Is<IEnumerable<Guid>>(ids => ids.Contains(inStockVariantId)),
                Arg.Any<CancellationToken>())
                .Returns(new List<BasketProductDto>
                {
                    new BasketProductDto(inStockVariantId, "In Stock Product", "White", "L", null, 30m, 30m, 5)
                });
            _mediator.Send(Arg.Any<GetCartQuery>(), Arg.Any<CancellationToken>())
                .Returns(new CartDto(Guid.NewGuid(), userId, null, new List<CartItemDto>()));
            _mediator.Send(Arg.Any<AddItemCommand>(), Arg.Any<CancellationToken>())
                .Returns(callInfo => _handler.Handle(callInfo.Arg<AddItemCommand>(), CancellationToken.None));
            Cart.Application.PublicApi.CartPublicApi cartPublicApi = new Cart.Application.PublicApi.CartPublicApi(_mediator);
            bool firstAdded = await cartPublicApi.AddItemAsync(userId, null, outOfStockVariantId, 1, CancellationToken.None);
            bool secondAdded = await cartPublicApi.AddItemAsync(userId, null, inStockVariantId, 1, CancellationToken.None);
            firstAdded.Should().BeFalse();
            secondAdded.Should().BeTrue();
            _dbContext.Carts.Received(1).Add(Arg.Any<CartEntity>());
        }
    }
}