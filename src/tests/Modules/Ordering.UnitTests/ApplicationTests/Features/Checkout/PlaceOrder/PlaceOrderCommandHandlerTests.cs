namespace Ordering.UnitTests.ApplicationTests.Features.Checkout.PlaceOrder
{
    public sealed class PlaceOrderCommandHandlerTests
    {
        private readonly IOrderingDbContext _dbContext;
        private readonly ICartPublicApi _cartApi;
        private readonly ICatalogPublicApi _catalogApi;
        private readonly IEventBus _eventBus;
        private readonly ILogger<PlaceOrderCommandHandler> _logger;
        private readonly PlaceOrderCommandHandler _handler;
        public PlaceOrderCommandHandlerTests()
        {
            _dbContext = Substitute.For<IOrderingDbContext>();
            _cartApi = Substitute.For<ICartPublicApi>();
            _catalogApi = Substitute.For<ICatalogPublicApi>();
            _eventBus = Substitute.For<IEventBus>();
            _logger = Substitute.For<ILogger<PlaceOrderCommandHandler>>();
            _handler = new PlaceOrderCommandHandler(_dbContext, _cartApi, _catalogApi, _eventBus, _logger);
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
            _catalogApi.TryReserveStockAsync(Arg.Any<Guid>(), Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(true);
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
            await act.Should().ThrowAsync<KeyNotFoundException>();
        }
        [Fact]
        public async Task Handle_ShouldPreventIdor_WhenAuthenticatedUserUsesGuestSessionId()
        {
            var guestSession = new CheckoutSession(null, "shared-session-id");
            var sessions = new List<CheckoutSession> { guestSession }.BuildMockDbSet();
            _dbContext.CheckoutSessions.Returns(sessions);
            var command = new PlaceOrderCommand(Guid.NewGuid(), "shared-session-id");
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
            await act.Should().ThrowAsync<KeyNotFoundException>();
        }
        [Fact]
        public async Task Handle_ShouldPreventIdor_WhenGuestUsesAuthenticatedUsersSessionId()
        {
            var ownerId = Guid.NewGuid();
            var userSession = new CheckoutSession(ownerId, "user-session-id");
            var sessions = new List<CheckoutSession> { userSession }.BuildMockDbSet();
            _dbContext.CheckoutSessions.Returns(sessions);
            var command = new PlaceOrderCommand(null, "user-session-id");
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
            await act.Should().ThrowAsync<KeyNotFoundException>();
        }
        [Fact]
        public async Task Handle_ShouldRollbackReservedStock_WhenSecondItemReservationFails()
        {
            var userId = Guid.NewGuid();
            var variantId1 = Guid.NewGuid();
            var variantId2 = Guid.NewGuid();
            var command = new PlaceOrderCommand(userId, "session");
            var session = new CheckoutSession(userId, "session");
            var sessions = new List<CheckoutSession> { session }.BuildMockDbSet();
            _dbContext.CheckoutSessions.Returns(sessions);
            var cartItems = new List<CartCheckoutItemDto>
            {
                new CartCheckoutItemDto(variantId1, "Product 1", null, 2, 25.0m),
                new CartCheckoutItemDto(variantId2, "Product 2", null, 3, 40.0m)
            };
            session.SetShippingInfo("t@t.com", "John", "Doe", "Address", "City", "Zip", "Standard", 5.99m);
            session.SetPaymentInfo("CreditCard", "1234");
            var cartInfo = new CartCheckoutInfoDto(cartItems, 170.0m, 0m);
            _cartApi.GetCartForCheckoutAsync(userId, "session", Arg.Any<CancellationToken>()).Returns(cartInfo);
            var basketProducts = new List<BasketProductDto>
            {
                new BasketProductDto(variantId1, "Product 1", "Black", "M", null, 25.0m, 25.0m, 10),
                new BasketProductDto(variantId2, "Product 2", "Blue", "L", null, 40.0m, 40.0m, 10)
            };
            _catalogApi.GetBasketProductsAsync(Arg.Any<IEnumerable<Guid>>(), Arg.Any<CancellationToken>()).Returns(basketProducts);
            _catalogApi.TryReserveStockAsync(variantId1, 2, Arg.Any<CancellationToken>()).Returns(true);
            _catalogApi.TryReserveStockAsync(variantId2, 3, Arg.Any<CancellationToken>()).Returns(false);
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
            await act.Should().ThrowAsync<InvalidOperationException>();
            await _catalogApi.Received(1).ReleaseStockAsync(variantId1, 2, Arg.Any<CancellationToken>());
            await _catalogApi.DidNotReceive().ReleaseStockAsync(variantId2, Arg.Any<int>(), Arg.Any<CancellationToken>());
        }
        [Fact]
        public async Task Handle_ShouldRollbackReservedStock_WhenSaveChangesThrowsException()
        {
            var userId = Guid.NewGuid();
            var variantId = Guid.NewGuid();
            var command = new PlaceOrderCommand(userId, "session");
            var session = new CheckoutSession(userId, "session");
            var sessions = new List<CheckoutSession> { session }.BuildMockDbSet();
            _dbContext.CheckoutSessions.Returns(sessions);
            var orders = new List<Order>().BuildMockDbSet();
            _dbContext.Orders.Returns(orders);
            var cartItems = new List<CartCheckoutItemDto>
            {
                new CartCheckoutItemDto(variantId, "Product 1", null, 2, 50.0m)
            };
            session.SetShippingInfo("t@t.com", "John", "Doe", "Address", "City", "Zip", "Standard", 5.99m);
            session.SetPaymentInfo("CreditCard", "1234");
            var cartInfo = new CartCheckoutInfoDto(cartItems, 100.0m, 0m);
            _cartApi.GetCartForCheckoutAsync(userId, "session", Arg.Any<CancellationToken>()).Returns(cartInfo);
            var basketProducts = new List<BasketProductDto>
            {
                new BasketProductDto(variantId, "Product 1", "Red", "M", null, 50.0m, 50.0m, 10)
            };
            _catalogApi.GetBasketProductsAsync(Arg.Any<IEnumerable<Guid>>(), Arg.Any<CancellationToken>()).Returns(basketProducts);
            _catalogApi.TryReserveStockAsync(variantId, 2, Arg.Any<CancellationToken>()).Returns(true);
            _dbContext.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns<Task<int>>(_ => throw new DbUpdateException("Database failure"));
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
            await act.Should().ThrowAsync<DbUpdateException>();
            await _catalogApi.Received(1).ReleaseStockAsync(variantId, 2, Arg.Any<CancellationToken>());
        }
        [Fact]
        public async Task Handle_ShouldThrowInvalidOperationException_WhenShippingOrPaymentInfoIsIncomplete()
        {
            Guid userId = Guid.NewGuid();
            PlaceOrderCommand command = new PlaceOrderCommand(userId, "session");
            CheckoutSession session = new CheckoutSession(userId, "session");
            DbSet<CheckoutSession> sessions = new List<CheckoutSession> { session }.BuildMockDbSet();
            _dbContext.CheckoutSessions.Returns(sessions);
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Shipping and payment information must be completed.");
        }
        [Fact]
        public async Task Handle_ShouldContinueReleasingRemainingStockAndRethrowOriginalException_WhenFirstReleaseThrows()
        {
            Guid userId = Guid.NewGuid();
            Guid variantId1 = Guid.NewGuid();
            Guid variantId2 = Guid.NewGuid();
            Guid variantId3 = Guid.NewGuid();
            PlaceOrderCommand command = new PlaceOrderCommand(userId, "session");
            CheckoutSession session = new CheckoutSession(userId, "session");
            session.SetShippingInfo("t@t.com", "John", "Doe", "Address", "City", "Zip", "Standard", 5.99m);
            session.SetPaymentInfo("CreditCard", "1234");
            DbSet<CheckoutSession> sessions = new List<CheckoutSession> { session }.BuildMockDbSet();
            _dbContext.CheckoutSessions.Returns(sessions);
            List<CartCheckoutItemDto> cartItems = new List<CartCheckoutItemDto>
            {
                new CartCheckoutItemDto(variantId1, "Product 1", null, 2, 25.0m),
                new CartCheckoutItemDto(variantId2, "Product 2", null, 3, 40.0m),
                new CartCheckoutItemDto(variantId3, "Product 3", null, 1, 15.0m)
            };
            CartCheckoutInfoDto cartInfo = new CartCheckoutInfoDto(cartItems, 185.0m, 0m);
            _cartApi.GetCartForCheckoutAsync(userId, "session", Arg.Any<CancellationToken>()).Returns(cartInfo);
            List<BasketProductDto> basketProducts = new List<BasketProductDto>
            {
                new BasketProductDto(variantId1, "Product 1", "Black", "M", null, 25.0m, 25.0m, 10),
                new BasketProductDto(variantId2, "Product 2", "Blue", "L", null, 40.0m, 40.0m, 10),
                new BasketProductDto(variantId3, "Product 3", "Green", "S", null, 15.0m, 15.0m, 10)
            };
            _catalogApi.GetBasketProductsAsync(Arg.Any<IEnumerable<Guid>>(), Arg.Any<CancellationToken>()).Returns(basketProducts);
            _catalogApi.TryReserveStockAsync(variantId1, 2, Arg.Any<CancellationToken>()).Returns(true);
            _catalogApi.TryReserveStockAsync(variantId2, 3, Arg.Any<CancellationToken>()).Returns(true);
            _catalogApi.TryReserveStockAsync(variantId3, 1, Arg.Any<CancellationToken>()).Returns(false);
            _catalogApi.ReleaseStockAsync(variantId1, 2, Arg.Any<CancellationToken>()).Returns(_ => Task.FromException(new DbUpdateException("Release failed for variant 1")));
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Insufficient stock for Product 3.");
            await _catalogApi.Received(1).ReleaseStockAsync(variantId1, 2, CancellationToken.None);
            await _catalogApi.Received(1).ReleaseStockAsync(variantId2, 3, CancellationToken.None);
            await _catalogApi.DidNotReceive().ReleaseStockAsync(variantId3, Arg.Any<int>(), Arg.Any<CancellationToken>());
        }
    }
}