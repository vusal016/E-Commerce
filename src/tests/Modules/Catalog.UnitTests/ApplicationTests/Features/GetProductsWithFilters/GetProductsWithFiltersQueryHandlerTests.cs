namespace Catalog.UnitTests.ApplicationTests.Features.GetProductsWithFilters
{
    public class GetProductsWithFiltersQueryHandlerTests
    {
        private readonly ICatalogDbContext _catalogDbContext;
        private readonly IPromotionsPublicApi _promotionsApi;
        private readonly GetProductsWithFiltersQueryHandler _handler;
        public GetProductsWithFiltersQueryHandlerTests()
        {
            _catalogDbContext = Substitute.For<ICatalogDbContext>();
            _promotionsApi = Substitute.For<IPromotionsPublicApi>();
            _handler = new GetProductsWithFiltersQueryHandler(_catalogDbContext, _promotionsApi);
        }
        [Fact]
        public async Task Handle_ShouldReturnProducts_WhenNoFiltersApplied()
        {
            var category = new Category("Electronics", "electronics", "icon");
            typeof(Category).GetProperty("Id")?.SetValue(category, Guid.NewGuid());
            var product = new Product("Smartphone", "Desc", 500, true, Guid.NewGuid(), category.Id);
            typeof(Product).GetProperty("Id")?.SetValue(product, Guid.NewGuid());
            var variant = new ProductVariant(product.Id, "Black", "M", "SKU", 500m, 550m, 10, true); typeof(ProductVariant).GetProperty("Id")?.SetValue(variant, Guid.NewGuid()); product.ProductVariants.Add(variant);
            var productsList = new List<Product> { product };
            var mockProductsDbSet = productsList.BuildMockDbSet();
            _catalogDbContext.Products.Returns(mockProductsDbSet);
            var categoriesList = new List<Category> { category };
            var mockCategoriesDbSet = categoriesList.BuildMockDbSet();
            _catalogDbContext.Categories.Returns(mockCategoriesDbSet);
            _promotionsApi.GetActiveDiscountsAsync(Arg.Any<List<Guid>>(), Arg.Any<CancellationToken>())
                .Returns(new List<ActiveDiscountDto>());
            var query = new GetProductsWithFiltersQuery(null, null, null, null, null, null, 1, 10);
            var result = await _handler.Handle(query, CancellationToken.None);
            result.Products.Should().HaveCount(1);
            result.Pagination.TotalCount.Should().Be(1);
            result.Products.First().Name.Should().Be("Smartphone");
        }
        [Fact]
        public async Task Handle_ShouldFilterByCategorySlug()
        {
            var cat1 = new Category("Cat1", "cat1", "icon");
            typeof(Category).GetProperty("Id")?.SetValue(cat1, Guid.NewGuid());
            var cat2 = new Category("Cat2", "cat2", "icon");
            typeof(Category).GetProperty("Id")?.SetValue(cat2, Guid.NewGuid());
            var product1 = new Product("P1", "Desc", 10, true, Guid.NewGuid(), cat1.Id);
            var product2 = new Product("P2", "Desc", 20, true, Guid.NewGuid(), cat2.Id);
            var productsList = new List<Product> { product1, product2 };
            var mockProductsDbSet = productsList.BuildMockDbSet();
            _catalogDbContext.Products.Returns(mockProductsDbSet);
            var categoriesList = new List<Category> { cat1, cat2 };
            var mockCategoriesDbSet = categoriesList.BuildMockDbSet();
            _catalogDbContext.Categories.Returns(mockCategoriesDbSet);
            _promotionsApi.GetActiveDiscountsAsync(Arg.Any<List<Guid>>(), Arg.Any<CancellationToken>())
                .Returns(new List<ActiveDiscountDto>());
            var query = new GetProductsWithFiltersQuery(null, "cat2", null, null, null, null, 1, 10);
            var result = await _handler.Handle(query, CancellationToken.None);
            result.Products.Should().HaveCount(1);
            result.Products.First().Name.Should().Be("P2");
        }
        [Fact]
        public async Task Handle_ShouldClampPageAndLimit_WhenZeroOrNegativeProvided()
        {
            Category category = new Category("Electronics", "electronics", "icon");
            typeof(Category).GetProperty("Id")?.SetValue(category, Guid.NewGuid());
            Product product = new Product("Smartphone", "Desc", 500, true, Guid.NewGuid(), category.Id);
            typeof(Product).GetProperty("Id")?.SetValue(product, Guid.NewGuid());
            List<Product> productsList = new List<Product> { product };
            DbSet<Product> mockProductsDbSet = productsList.BuildMockDbSet();
            _catalogDbContext.Products.Returns(mockProductsDbSet);
            List<Category> categoriesList = new List<Category> { category };
            DbSet<Category> mockCategoriesDbSet = categoriesList.BuildMockDbSet();
            _catalogDbContext.Categories.Returns(mockCategoriesDbSet);
            _promotionsApi.GetActiveDiscountsAsync(Arg.Any<List<Guid>>(), Arg.Any<CancellationToken>())
                .Returns(new List<ActiveDiscountDto>());
            GetProductsWithFiltersQuery query = new GetProductsWithFiltersQuery(null, null, null, null, null, null, 0, 0);
            Func<Task<(List<GetProductsWithFiltersDto> Products, PaginationInfo Pagination)>> act = async () => await _handler.Handle(query, CancellationToken.None);
            (List<GetProductsWithFiltersDto> Products, PaginationInfo Pagination) result = (await act.Should().NotThrowAsync()).Subject;
            result.Pagination.CurrentPage.Should().Be(1);
            result.Pagination.Limit.Should().Be(1);
            result.Pagination.TotalPages.Should().Be(1);
            result.Products.Should().HaveCount(1);
        }
    }
}