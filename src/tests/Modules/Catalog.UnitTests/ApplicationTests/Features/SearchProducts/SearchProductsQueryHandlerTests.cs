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
        [Theory]
        [InlineData(0)]
        [InlineData(-3)]
        public async Task Handle_WhenPageIsZeroOrNegative_ShouldNotThrowAndShouldClampCurrentPageToOne(int invalidPage)
        {
            Brand brand = new Brand("Nike", "nike", "logo", "url");
            typeof(Brand).GetProperty("Id")?.SetValue(brand, Guid.NewGuid());
            typeof(Brand).GetProperty("IsActive")?.SetValue(brand, true);
            Product product = new Product("Air Max", "Shoes", 120, true, brand.Id, Guid.NewGuid());
            DbSet<Product> mockProductsDbSet = new List<Product> { product }.BuildMockDbSet();
            _catalogDbContext.Products.Returns(mockProductsDbSet);
            DbSet<Brand> mockBrandsDbSet = new List<Brand> { brand }.BuildMockDbSet();
            _catalogDbContext.Brands.Returns(mockBrandsDbSet);
            SearchProductsQuery query = new SearchProductsQuery("air", invalidPage, 10);
            Func<Task<(SearchResponseDto Data, PaginationInfo Pagination)>> act = async () => await _handler.Handle(query, CancellationToken.None);
            await act.Should().NotThrowAsync();
            (SearchResponseDto Data, PaginationInfo Pagination) result = await _handler.Handle(query, CancellationToken.None);
            result.Pagination.CurrentPage.Should().Be(1);
            SearchProductsQuery emptyQuery = new SearchProductsQuery("", invalidPage, 10);
            (SearchResponseDto Data, PaginationInfo Pagination) emptyResult = await _handler.Handle(emptyQuery, CancellationToken.None);
            emptyResult.Pagination.CurrentPage.Should().Be(1);
        }
    }
}