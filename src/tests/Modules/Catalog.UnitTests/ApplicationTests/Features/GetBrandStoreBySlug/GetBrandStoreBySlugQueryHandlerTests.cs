namespace Catalog.UnitTests.ApplicationTests.Features.GetBrandStoreBySlug
{
    public class GetBrandStoreBySlugQueryHandlerTests
    {
        private readonly ICatalogDbContext _catalogDbContext;
        private readonly GetBrandStoreBySlugQueryHandler _handler;

        public GetBrandStoreBySlugQueryHandlerTests()
        {
            _catalogDbContext = Substitute.For<ICatalogDbContext>();
            _handler = new GetBrandStoreBySlugQueryHandler(_catalogDbContext);
        }

        [Fact]
        public async Task Handle_ShouldReturnBrandStore_WhenBrandExists()
        {
            var brand = new Brand("Nike", "nike", "logo", "desc");
            typeof(Brand).GetProperty("Id")?.SetValue(brand, Guid.NewGuid());
            typeof(Brand).GetProperty("IsActive")?.SetValue(brand, true);

            var product = new Product("Air Force 1", "Shoes", 120, true, brand.Id, Guid.NewGuid());
            var productTag = new ProductTag(product.Id, Catalog.Domain.Enums.TagType.BestSeller, "BestSeller");
            
            var tagsProp = typeof(Product).GetProperty("ProductTags");
            var tagsList = new List<ProductTag> { productTag };
            tagsProp?.SetValue(product, tagsList);

            var variant = new ProductVariant(product.Id, "Black", "US 10", "SKU1", 120, 150, 10, true);
            var variantsProp = typeof(Product).GetProperty("ProductVariants");
            var variantsList = new List<ProductVariant> { variant };
            variantsProp?.SetValue(product, variantsList);

            var brandsList = new List<Brand> { brand };
            var mockBrandsDbSet = brandsList.BuildMockDbSet();
            _catalogDbContext.Brands.Returns(mockBrandsDbSet);

            var productsList = new List<Product> { product };
            var mockProductsDbSet = productsList.BuildMockDbSet();
            _catalogDbContext.Products.Returns(mockProductsDbSet);

            var query = new GetBrandStoreBySlugQuery("nike");

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Brand.Name.Should().Be("Nike");
            result.Products.Should().HaveCount(1);
            result.BestSellers.Should().HaveCount(1);
            result.BestSellers.First().Name.Should().Be("Air Force 1");
            result.Filters.AvailableColors.Should().Contain("Black");
            result.Filters.AvailableSizes.Should().Contain("US 10");
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenBrandDoesNotExist()
        {
            var brandsList = new List<Brand>();
            var mockBrandsDbSet = brandsList.BuildMockDbSet();
            _catalogDbContext.Brands.Returns(mockBrandsDbSet);

            var query = new GetBrandStoreBySlugQuery("unknown");

            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            await act.Should().ThrowAsync<KeyNotFoundException>();
        }
    }
}
