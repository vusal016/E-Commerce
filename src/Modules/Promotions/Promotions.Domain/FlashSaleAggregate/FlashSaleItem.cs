namespace Promotions.Domain.FlashSaleAggregate
{
    public sealed class FlashSaleItem : AuditEntity
    {
        private FlashSaleItem()
        {
        }

        public FlashSaleItem(Guid flashSaleId, Guid productVariantId, decimal discountPercentage, int soldCount, int stockLimit)
        {
            SetFlashSaleId(flashSaleId);
            SetProductVariantId(productVariantId);
            SetDiscountPercentage(discountPercentage);
            SetSoldCount(soldCount);
            SetStockLimit(stockLimit);
        }

        public Guid FlashSaleId { get; private set; }
        public Guid ProductVariantId { get; private set; }
        public decimal DiscountPercentage { get; private set; }
        public int SoldCount { get; private set; }
        public int StockLimit { get; private set; }

        public FlashSale FlashSale { get; private set; } = null!;

        private void SetFlashSaleId(Guid flashSaleId)
        {
            if (flashSaleId == Guid.Empty)
                throw new ArgumentException("Flash sale ID cannot be empty.");
            FlashSaleId = flashSaleId;
        }

        private void SetProductVariantId(Guid productVariantId)
        {
            if (productVariantId == Guid.Empty)
                throw new ArgumentException("Product variant ID cannot be empty.");
            ProductVariantId = productVariantId;
        }

        private void SetDiscountPercentage(decimal discountPercentage)
        {
            if (discountPercentage <= 0 || discountPercentage > 100)
                throw new ArgumentException("Discount percentage must be between 0 and 100.");
            DiscountPercentage = discountPercentage;
        }

        private void SetSoldCount(int soldCount)
        {
            if (soldCount < 0)
                throw new ArgumentException("Sold count cannot be negative.");
            SoldCount = soldCount;
        }

        private void SetStockLimit(int stockLimit)
        {
            if (stockLimit < 0)
                throw new ArgumentException("Stock limit cannot be negative.");
            StockLimit = stockLimit;
        }
    }
}
