namespace Catalog.UnitTests.ApplicationTests.Features.GetProductById;

public class GetProductByIdQueryHandlerTests
{
    private readonly ICatalogDbContext _catalogDbContext;
    private readonly IEngagementPublicApi _engagementApi;
    private readonly IPromotionsPublicApi _promotionsApi;
    private readonly GetProductByIdQueryHandler _handler;

    public GetProductByIdQueryHandlerTests()
    {
        _catalogDbContext = Substitute.For<ICatalogDbContext>();
        _engagementApi = Substitute.For<IEngagementPublicApi>();
        _promotionsApi = Substitute.For<IPromotionsPublicApi>();
        _handler = new GetProductByIdQueryHandler(_catalogDbContext, _engagementApi, _promotionsApi);
    }

    [Fact]
    public async Task Handle_ShouldReturnProduct_WhenProductExistsAndIsActive()
    {
        var categoryId = Guid.NewGuid();
        var brandId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var product = new Product("Test Product", "Test Description", 100, true, brandId, categoryId);
        typeof(Product).GetProperty("Id")?.SetValue(product, productId);

        var productsList = new List<Product> { product };
        var mockDbSet = productsList.BuildMockDbSet();

        _catalogDbContext.Products.Returns(mockDbSet);
        
        _engagementApi.GetProductRatingAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(new ProductRatingDto(productId, 4.5m, 10));
            
        _promotionsApi.GetActiveDiscountsAsync(Arg.Any<IEnumerable<Guid>>(), Arg.Any<CancellationToken>())
            .Returns(new List<ActiveDiscountDto>());

        var query = new GetProductByIdQuery(productId);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Id.Should().Be(productId);
        result.Name.Should().Be("Test Product");
        result.RatingAverage.Should().Be(4.5m);
        result.ReviewCount.Should().Be(10);
        result.RelatedProducts.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldThrowKeyNotFoundException_WhenProductDoesNotExist()
    {
        var mockDbSet = new List<Product>().BuildMockDbSet();
        _catalogDbContext.Products.Returns(mockDbSet);

        var query = new GetProductByIdQuery(Guid.NewGuid());

        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Product not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnRelatedProducts_WhenSameCategoryExists()
    {
        var categoryId = Guid.NewGuid();
        var brandId = Guid.NewGuid();
        var productId1 = Guid.NewGuid();
        var productId2 = Guid.NewGuid();

        var product1 = new Product("Target Product", "Desc", 100, true, brandId, categoryId);
        typeof(Product).GetProperty("Id")?.SetValue(product1, productId1);

        var product2 = new Product("Related Product", "Desc", 200, true, brandId, categoryId);
        typeof(Product).GetProperty("Id")?.SetValue(product2, productId2);

        var productsList = new List<Product> { product1, product2 };
        var mockDbSet = productsList.BuildMockDbSet();

        _catalogDbContext.Products.Returns(mockDbSet);
        
        _engagementApi.GetProductRatingAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(new ProductRatingDto(productId1, 4.5m, 10));
            
        _promotionsApi.GetActiveDiscountsAsync(Arg.Any<IEnumerable<Guid>>(), Arg.Any<CancellationToken>())
            .Returns(new List<ActiveDiscountDto>());

        var query = new GetProductByIdQuery(productId1);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.RelatedProducts.Should().HaveCount(1);
        result.RelatedProducts.First().Id.Should().Be(productId2);
        result.RelatedProducts.First().Name.Should().Be("Related Product");
    }
}
