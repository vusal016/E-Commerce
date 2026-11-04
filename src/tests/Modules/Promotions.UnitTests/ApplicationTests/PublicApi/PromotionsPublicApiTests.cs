namespace Promotions.UnitTests.ApplicationTests.PublicApi
{
    public sealed class PromotionsPublicApiTests
    {
        private readonly IPromotionsDbContext _dbContext;
        private readonly PromotionsPublicApi _sut;
        public PromotionsPublicApiTests()
        {
            _dbContext = Substitute.For<IPromotionsDbContext>();
            _sut = new PromotionsPublicApi(_dbContext);
        }
        [Fact]
        public async Task GetActiveDiscountsAsync_WhenVariantIsInTwoActiveFlashSales_ShouldReturnSingleHighestDiscount()
        {
            Guid variantId = Guid.NewGuid();
            FlashSale sale1 = new FlashSale("Sale 1", DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1), true);
            sale1.Items.Add(new FlashSaleItem(sale1.Id, variantId, 10m, 0, 50));
            FlashSale sale2 = new FlashSale("Sale 2", DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1), true);
            sale2.Items.Add(new FlashSaleItem(sale2.Id, variantId, 25m, 0, 50));
            List<FlashSale> sales = new List<FlashSale> { sale1, sale2 };
            DbSet<FlashSale> mockDbSet = sales.BuildMockDbSet();
            _dbContext.FlashSales.Returns(mockDbSet);
            List<ActiveDiscountDto> result = await _sut.GetActiveDiscountsAsync(new List<Guid> { variantId }, CancellationToken.None);
            result.Should().HaveCount(1);
            result[0].ProductVariantId.Should().Be(variantId);
            result[0].DiscountPercentage.Should().Be(25m);
        }
    }
}
