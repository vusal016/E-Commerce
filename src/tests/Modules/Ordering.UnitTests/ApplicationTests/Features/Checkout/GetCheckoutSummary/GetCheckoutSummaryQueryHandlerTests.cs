namespace Ordering.UnitTests.ApplicationTests.Features.Checkout.GetCheckoutSummary;

public sealed class GetCheckoutSummaryQueryHandlerTests
{
    private readonly IOrderingDbContext _dbContext;
    private readonly ICartPublicApi _cartApi;
    private readonly GetCheckoutSummaryQueryHandler _handler;

    public GetCheckoutSummaryQueryHandlerTests()
    {
        _dbContext = Substitute.For<IOrderingDbContext>();
        _cartApi = Substitute.For<ICartPublicApi>();
        _handler = new GetCheckoutSummaryQueryHandler(_dbContext, _cartApi);
    }

    [Fact]
    public async Task Handle_ShouldReturnSummary_WhenSessionAndCartExist()
    {
        var userId = Guid.NewGuid();
        var query = new GetCheckoutSummaryQuery(userId, "session");

        var session = new CheckoutSession(userId, "session");
        session.SetShippingInfo("t@t.com", "John", "Doe", "Address", "City", "Zip", "Standard", 5.99m);
        session.SetPaymentInfo("CreditCard", "1234");
        
        var sessions = new List<CheckoutSession> { session }.BuildMockDbSet();
        _dbContext.CheckoutSessions.Returns(sessions);

        var cartItems = new List<CartCheckoutItemDto> 
        { 
            new CartCheckoutItemDto(Guid.NewGuid(), "Product A", null, 2, 50.0m) 
        };
        var cartInfo = new CartCheckoutInfoDto(cartItems, 100.0m, 10.0m);
        _cartApi.GetCartForCheckoutAsync(userId, "session", Arg.Any<CancellationToken>()).Returns(cartInfo);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Subtotal.Should().Be(100.0m);
        result.CartDiscount.Should().Be(10.0m);
        result.ShippingPrice.Should().Be(5.99m);
        result.TotalPrice.Should().Be(95.99m);
        result.Items.Should().HaveCount(1);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenSessionNotFound()
    {
        var query = new GetCheckoutSummaryQuery(Guid.NewGuid(), "session");
        var sessions = new List<CheckoutSession>().BuildMockDbSet();
        _dbContext.CheckoutSessions.Returns(sessions);

        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenCartIsEmpty()
    {
        var userId = Guid.NewGuid();
        var query = new GetCheckoutSummaryQuery(userId, "session");
        
        var session = new CheckoutSession(userId, "session");
        var sessions = new List<CheckoutSession> { session }.BuildMockDbSet();
        _dbContext.CheckoutSessions.Returns(sessions);

        _cartApi.GetCartForCheckoutAsync(userId, "session", Arg.Any<CancellationToken>()).Returns((CartCheckoutInfoDto?)null);

        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
