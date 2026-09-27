namespace Promotions.Application.Features.FlashSales.Queries.GetActiveFlashSales
{
    public sealed record GetActiveFlashSalesQuery : ICachedQuery<IReadOnlyList<FlashSaleDto>>
    {
        public string Key => "ActiveFlashSales";
        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);
    }
}
