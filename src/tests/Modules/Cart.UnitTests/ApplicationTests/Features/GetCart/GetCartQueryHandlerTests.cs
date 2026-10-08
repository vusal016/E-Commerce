namespace Cart.UnitTests.ApplicationTests.Features.GetCart;

public sealed class GetCartQueryHandlerTests
{
    private readonly ICartDbContext _dbContext;
    private readonly ICatalogPublicApi _catalogPublicApi;
    private readonly IPromotionsPublicApi _promotionsApi;
    private readonly GetCartQueryHandler _handler;

    public GetCartQueryHandlerTests()
    {
        _dbContext = Substitute.For<ICartDbContext>();
        _catalogPublicApi = Substitute.For<ICatalogPublicApi>();
        _promotionsApi = Substitute.For<IPromotionsPublicApi>();
        _handler = new GetCartQueryHandler(_dbContext, _catalogPublicApi, _promotionsApi);
    }

    [Fact]
    public async Task Handle_ShouldThrowKeyNotFoundException_WhenCartDoesNotExist()
    {
        var query = new GetCartQuery(Guid.NewGuid(), "session-123");
        var carts = new List<Cart.Domain.CartAggregate.Cart>().BuildMockDbSet();
        _dbContext.Carts.Returns(carts);

        await FluentActions.Invoking(() => _handler.Handle(query, CancellationToken.None))
            .Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Basket not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnCartDto_WhenCartExists()
    {
        var userId = Guid.NewGuid();
        var sessionId = "session-123";
        var query = new GetCartQuery(userId, sessionId);
        
        var cart = new Cart.Domain.CartAggregate.Cart(userId, sessionId);
        
        // Use reflection to set Id
        typeof(Cart.Domain.CartAggregate.Cart).GetProperty("Id")?.SetValue(cart, Guid.NewGuid());
        
        var variantId1 = Guid.NewGuid();
        var variantId2 = Guid.NewGuid();
        
        var cartItem1 = new Cart.Domain.CartAggregate.CartItem(cart.Id, variantId1, 2, false);
        typeof(Cart.Domain.CartAggregate.CartItem).GetProperty("Id")?.SetValue(cartItem1, Guid.NewGuid());
        var cartItem2 = new Cart.Domain.CartAggregate.CartItem(cart.Id, variantId2, 1, false);
        typeof(Cart.Domain.CartAggregate.CartItem).GetProperty("Id")?.SetValue(cartItem2, Guid.NewGuid());
        
        cart.Items.Add(cartItem1);
        cart.Items.Add(cartItem2);

        var carts = new List<Cart.Domain.CartAggregate.Cart> { cart }.BuildMockDbSet();
        _dbContext.Carts.Returns(carts);

        var basketProducts = new List<BasketProductDto>
        {
            new BasketProductDto(variantId1, "Product 1", "Red", "L", "img1.jpg", 10.0m, 12.0m, 20),
            new BasketProductDto(variantId2, "Product 2", "Blue", "M", "img2.jpg", 15.0m, 15.0m, 3)
        };

        _catalogPublicApi.GetBasketProductsAsync(Arg.Any<IEnumerable<Guid>>(), Arg.Any<CancellationToken>())
            .Returns(basketProducts);
            
        var discounts = new List<ActiveDiscountDto>();
        _promotionsApi.GetActiveDiscountsAsync(Arg.Any<IEnumerable<Guid>>(), Arg.Any<CancellationToken>())
            .Returns(discounts);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Id.Should().Be(cart.Id);
        result.UserId.Should().Be(userId);
        result.SessionId.Should().Be(sessionId);
        result.Items.Should().HaveCount(2);

        var item1 = result.Items.Single(i => i.ProductVariantId == variantId1);
        item1.ProductName.Should().Be("Product 1");
        item1.Price.Should().Be(10.0m);
        item1.Quantity.Should().Be(2);
        item1.StockWarning.Should().BeNull();

        var item2 = result.Items.Single(i => i.ProductVariantId == variantId2);
        item2.ProductName.Should().Be("Product 2");
        item2.Price.Should().Be(15.0m);
        item2.Quantity.Should().Be(1);
        item2.StockWarning.Should().Be("Only 3 left");
    }
}


