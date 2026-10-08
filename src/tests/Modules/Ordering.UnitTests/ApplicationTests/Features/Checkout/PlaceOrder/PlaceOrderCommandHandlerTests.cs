namespace Ordering.UnitTests.ApplicationTests.Features.Checkout.PlaceOrder;

public sealed class PlaceOrderCommandHandlerTests
{
    private readonly IOrderingDbContext _dbContext;
    private readonly ICartPublicApi _cartApi;
    private readonly ICatalogPublicApi _catalogApi;
    private readonly IEventBus _eventBus;
    private readonly PlaceOrderCommandHandler _handler;

    public PlaceOrderCommandHandlerTests()
    {
        _dbContext = Substitute.For<IOrderingDbContext>();
        _cartApi = Substitute.For<ICartPublicApi>();
        _catalogApi = Substitute.For<ICatalogPublicApi>();
        _eventBus = Substitute.For<IEventBus>();
        _handler = new PlaceOrderCommandHandler(_dbContext, _cartApi, _catalogApi, _eventBus);
    }

    [Fact]
    public async Task Handle_ShouldCreateOrderAndPublishEvent_WhenSessionAndCartExist()
    {
        var userId = Guid.NewGuid();
        var command = new PlaceOrderCommand(userId, "session");

        var session = new CheckoutSession(userId, "session");
        session.SetShippingInfo("t@t.com", "John", "Doe", "Address", "City", "Zip", "Standard", 5.99m);
        session.SetPaymentInfo("CreditCard", "1234");
        
        var sessions = new List<CheckoutSession> { session }.BuildMockDbSet();
        _dbContext.CheckoutSessions.Returns(sessions);

        var orders = new List<Order>().BuildMockDbSet();
        _dbContext.Orders.Returns(orders);

        var cartItems = new List<CartCheckoutItemDto> 
        { 
            new CartCheckoutItemDto(Guid.NewGuid(), "Product A", null, 2, 50.0m) 
        };
        var cartInfo = new CartCheckoutInfoDto(cartItems, 100.0m, 10.0m);
        _cartApi.GetCartForCheckoutAsync(userId, "session", Arg.Any<CancellationToken>()).Returns(cartInfo);

        var basketProducts = new List<BasketProductDto>
        {
            new BasketProductDto(cartItems[0].ProductVariantId, "Product A", "Red", "M", null, 50.0m, 50.0m, 10)
        };
        _catalogApi.GetBasketProductsAsync(Arg.Any<IEnumerable<Guid>>(), Arg.Any<CancellationToken>()).Returns(basketProducts);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeEmpty();
        _dbContext.Orders.Received(1).Add(Arg.Is<Order>(o => o.Total == 95.99m && o.Items.Count == 1));
        session.IsCompleted.Should().BeTrue();
        await _dbContext.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        await _eventBus.Received(1).PublishAsync(Arg.Is<OrderPlacedIntegrationEvent>(e => e.UserId == userId && e.Items.Count == 1), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenSessionNotFound()
    {
        var command = new PlaceOrderCommand(Guid.NewGuid(), "session");
        var sessions = new List<CheckoutSession>().BuildMockDbSet();
        _dbContext.CheckoutSessions.Returns(sessions);

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenSessionIsCompleted()
    {
        var userId = Guid.NewGuid();
        var command = new PlaceOrderCommand(userId, "session");

        var session = new CheckoutSession(userId, "session");
        session.MarkAsCompleted();

        var sessions = new List<CheckoutSession> { session }.BuildMockDbSet();
        _dbContext.CheckoutSessions.Returns(sessions);

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Checkout session is already completed.");
    }
}
