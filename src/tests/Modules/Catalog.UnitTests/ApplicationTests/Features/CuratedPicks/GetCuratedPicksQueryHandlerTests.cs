namespace Catalog.UnitTests.ApplicationTests.Features.CuratedPicks
{
    public class GetCuratedPicksQueryHandlerTests
    {
        private readonly ICatalogDbContext _catalogDbContext;
        private readonly Catalog.Application.Features.CuratedPicks.Queries.GetCuratedPicksQueryHandler _handler;

        public GetCuratedPicksQueryHandlerTests()
        {
            _catalogDbContext = Substitute.For<ICatalogDbContext>();
            _handler = new Catalog.Application.Features.CuratedPicks.Queries.GetCuratedPicksQueryHandler(_catalogDbContext);
        }

        [Fact]
        public async Task Handle_ShouldReturnOnlyFeaturedProducts()
        {
            var product1 = new Product("Featured Product", "Desc", 100, true, Guid.NewGuid(), Guid.NewGuid());
            var productTag1 = new ProductTag(product1.Id, Catalog.Domain.Enums.TagType.Featured, "Featured");
            var tagsProp1 = typeof(Product).GetProperty("ProductTags");
            tagsProp1?.SetValue(product1, new List<ProductTag> { productTag1 });

            var product2 = new Product("Normal Product", "Desc", 50, true, Guid.NewGuid(), Guid.NewGuid());
            
            var productsList = new List<Product> { product1, product2 };
            var mockProductsDbSet = productsList.BuildMockDbSet();
            _catalogDbContext.Products.Returns(mockProductsDbSet);

            var query = new Catalog.Application.Features.CuratedPicks.Queries.GetCuratedPicksQuery();
            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().HaveCount(1);
            result.First().Name.Should().Be("Featured Product");
        }
    }
}
