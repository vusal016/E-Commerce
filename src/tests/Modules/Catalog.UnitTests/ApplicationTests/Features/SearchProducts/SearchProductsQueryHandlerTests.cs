namespace Catalog.UnitTests.ApplicationTests.Features.SearchProducts
{
    public class SearchProductsQueryHandlerTests
    {
        private readonly ICatalogDbContext _catalogDbContext;
        private readonly SearchProductsQueryHandler _handler;

        public SearchProductsQueryHandlerTests()
        {
            _catalogDbContext = Substitute.For<ICatalogDbContext>();
            _handler = new SearchProductsQueryHandler(_catalogDbContext);
        }

        [Fact]
        public async Task Handle_ShouldReturnMatchingProducts_WhenQueryMatchesProductName()
        {
            var brand = new Brand("Nike", "nike", "logo", "url");
            typeof(Brand).GetProperty("Id")?.SetValue(brand, Guid.NewGuid());
            typeof(Brand).GetProperty("IsActive")?.SetValue(brand, true);
            
            var brand2 = new Brand("Samsung", "samsung", "logo", "url");
            typeof(Brand).GetProperty("Id")?.SetValue(brand2, Guid.NewGuid());
            typeof(Brand).GetProperty("IsActive")?.SetValue(brand2, true);

            var product1 = new Product("Air Max", "Shoes", 120, true, brand.Id, Guid.NewGuid());
            var product2 = new Product("Galaxy S25", "Phone", 900, true, brand2.Id, Guid.NewGuid());

            var productsList = new List<Product> { product1, product2 };
            var mockProductsDbSet = productsList.BuildMockDbSet();
            _catalogDbContext.Products.Returns(mockProductsDbSet);

            var brandsList = new List<Brand> { brand, brand2 };
            var mockBrandsDbSet = brandsList.BuildMockDbSet();
            _catalogDbContext.Brands.Returns(mockBrandsDbSet);

            var query = new SearchProductsQuery("air");

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Data.Products.Should().HaveCount(1);
            result.Data.Products.First().Name.Should().Be("Air Max");
            result.Data.SuggestedQuery.Should().BeNull();
        }

        [Fact]
        public async Task Handle_ShouldReturnSuggestedQuery_WhenNoProductsMatchButSimilarNameExists()
        {
            var brand = new Brand("Samsung", "samsung", "logo", "url");
            typeof(Brand).GetProperty("Id")?.SetValue(brand, Guid.NewGuid());
            typeof(Brand).GetProperty("IsActive")?.SetValue(brand, true);

            var product = new Product("Galaxy S25", "Phone", 900, true, brand.Id, Guid.NewGuid());
            var productsList = new List<Product> { product };
            var mockProductsDbSet = productsList.BuildMockDbSet();
            _catalogDbContext.Products.Returns(mockProductsDbSet);

            var brandsList = new List<Brand> { brand };
            var mockBrandsDbSet = brandsList.BuildMockDbSet();
            _catalogDbContext.Brands.Returns(mockBrandsDbSet);

            var query = new SearchProductsQuery("samxy");

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Data.Products.Should().BeEmpty();
            result.Data.SuggestedQuery.Should().Be("Samsung");
        }
    }
}
