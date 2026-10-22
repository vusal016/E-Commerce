namespace SharedKernel.Pricing
{
    public static class PriceCalculator
    {
        public static decimal CalculateDiscountedPrice(decimal price, decimal discountPercentage)
        {
            return Math.Round(Math.Max(0, price - (price * discountPercentage / 100m)), 2);
        }
    }
}
