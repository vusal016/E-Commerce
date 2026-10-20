using Cart.Application.PublicApi;

namespace Cart.UnitTests.ApplicationTests.PublicApi
{
    public sealed class CartPublicApiTests
    {
        [Fact]
        public async Task AddItemAsync_ShouldReturnFalse_WhenInsufficientStockThrowsArgumentException()
        {
            IMediator mediator = Substitute.For<IMediator>();
            mediator.Send(Arg.Any<AddItemCommand>(), Arg.Any<CancellationToken>())
                .Returns<Task<CartDto>>(_ => throw new ArgumentException("Not enough stock available."));
            CartPublicApi sut = new CartPublicApi(mediator);
            bool result = await sut.AddItemAsync(Guid.NewGuid(), "session", Guid.NewGuid(), 5, CancellationToken.None);
            result.Should().BeFalse();
        }
    }
}