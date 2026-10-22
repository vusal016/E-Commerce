namespace Promotions.UnitTests.ApplicationTests.Pricing
{
    public sealed class PriceCalculatorTests
    {
        [Fact]
        public void CalculateDiscountedPrice_WhenNormalDiscount_ShouldReturnDiscountedPrice()
        {
            decimal result = PriceCalculator.CalculateDiscountedPrice(100m, 20m);
            result.Should().Be(80m);
        }
        [Fact]
        public void CalculateDiscountedPrice_WhenZeroDiscount_ShouldReturnOriginalPrice()
        {
            decimal result = PriceCalculator.CalculateDiscountedPrice(100m, 0m);
            result.Should().Be(100m);
        }
        [Fact]
        public void CalculateDiscountedPrice_WhenDiscountExceedsOneHundredPercent_ShouldReturnZero()
        {
            decimal result = PriceCalculator.CalculateDiscountedPrice(100m, 120m);
            result.Should().Be(0m);
        }
        [Fact]
        public void CalculateDiscountedPrice_WhenResultHasMoreThanTwoDecimals_ShouldRoundToTwoDecimals()
        {
            decimal result = PriceCalculator.CalculateDiscountedPrice(19.99m, 15m);
            result.Should().Be(16.99m);
        }
    }
}
