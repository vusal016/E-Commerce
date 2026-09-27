namespace Promotions.UnitTests.ApplicationTests.Features.FlashSales.Queries.GetActiveFlashSales;

public class GetActiveFlashSalesQueryHandlerTests
{
    private readonly IPromotionsDbContext _dbContext;
    private readonly ICatalogPublicApi _catalogApi;
    private readonly GetActiveFlashSalesQueryHandler _handler;

    public GetActiveFlashSalesQueryHandlerTests()
    {
        _dbContext = Substitute.For<IPromotionsDbContext>();
        _catalogApi = Substitute.For<ICatalogPublicApi>();
        _handler = new GetActiveFlashSalesQueryHandler(_dbContext, _catalogApi);
    }

    [Fact]
    public async Task Handle_ShouldReturnActiveFlashSales_WithCorrectCalculations()
    {
        // Arrange
        var variantId = Guid.NewGuid();
        var flashSaleId = Guid.NewGuid();
        
        var flashSale = new FlashSale("Test Sale", DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1), true);
        typeof(FlashSale).GetProperty("Id")?.SetValue(flashSale, flashSaleId);
        
        var item = new FlashSaleItem(flashSaleId, variantId, 20m, 10, 50); // 20% discount, 10 sold, 50 limit (20% sold)
        typeof(FlashSaleItem).GetProperty("Id")?.SetValue(item, Guid.NewGuid());
        flashSale.Items.Add(item);

        var list = new List<FlashSale> { flashSale };
        var mockDbSet = list.BuildMockDbSet();
        _dbContext.FlashSales.Returns(mockDbSet);

        // Mock Catalog API
        var productSummaries = new List<ProductSummaryDto>
        {
            new ProductSummaryDto(Guid.NewGuid(),  variantId,  "Sneaker",  "https://img.com",  100m, 10) // 100 Base price
        };
        
        _catalogApi.GetProductSummariesAsync(Arg.Any<IEnumerable<Guid>>(), Arg.Any<CancellationToken>())
            .Returns(productSummaries);

        // Act
        var result = await _handler.Handle(new GetActiveFlashSalesQuery(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        
        var returnedSale = result.First();
        returnedSale.Items.Should().HaveCount(1);
        
        var returnedItem = returnedSale.Items.First();
        returnedItem.ProductName.Should().Be("Sneaker");
        returnedItem.OriginalPrice.Should().Be(100m);
        returnedItem.DiscountedPrice.Should().Be(80m); // 100 - 20%
        returnedItem.SoldPercentage.Should().Be(20.0); // 10 / 50 * 100
    }
}


