namespace Promotions.UnitTests.ApplicationTests.Features.FlashSales.Queries.GetUpcomingFlashSales;

public class GetUpcomingFlashSalesQueryHandlerTests
{
    private readonly IPromotionsDbContext _dbContext;
    private readonly ICatalogPublicApi _catalogApi;
    private readonly GetUpcomingFlashSalesQueryHandler _handler;

    public GetUpcomingFlashSalesQueryHandlerTests()
    {
        _dbContext = Substitute.For<IPromotionsDbContext>();
        _catalogApi = Substitute.For<ICatalogPublicApi>();
        _handler = new GetUpcomingFlashSalesQueryHandler(_dbContext, _catalogApi);
    }

    [Fact]
    public async Task Handle_ShouldReturnUpcomingFlashSales_WithCorrectCalculations()
    {
        // Arrange
        var variantId = Guid.NewGuid();
        var flashSaleId = Guid.NewGuid();
        
        var flashSale = new FlashSale("Upcoming Sale", DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(3), true);
        typeof(FlashSale).GetProperty("Id")?.SetValue(flashSale, flashSaleId);
        
        var item = new FlashSaleItem(flashSaleId, variantId, 15m, 0, 100); // 15% discount, 0 sold, 100 limit (0% sold)
        typeof(FlashSaleItem).GetProperty("Id")?.SetValue(item, Guid.NewGuid());
        flashSale.Items.Add(item);

        var list = new List<FlashSale> { flashSale };
        var mockDbSet = list.BuildMockDbSet();
        _dbContext.FlashSales.Returns(mockDbSet);

        // Mock Catalog API
        var productSummaries = new List<ProductSummaryDto>
        {
            new ProductSummaryDto(Guid.NewGuid(),  variantId,  "T-Shirt",  "https://img2.com",  200m, 10) // 200 Base price
        };
        
        _catalogApi.GetProductSummariesAsync(Arg.Any<IEnumerable<Guid>>(), Arg.Any<CancellationToken>())
            .Returns(productSummaries);

        // Act
        var result = await _handler.Handle(new GetUpcomingFlashSalesQuery(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        
        var returnedSale = result.First();
        returnedSale.Items.Should().HaveCount(1);
        
        var returnedItem = returnedSale.Items.First();
        returnedItem.ProductName.Should().Be("T-Shirt");
        returnedItem.OriginalPrice.Should().Be(200m);
        returnedItem.DiscountedPrice.Should().Be(170m); // 200 - 15%
        returnedItem.SoldPercentage.Should().Be(0); // 0 / 100 * 100
    }
}


