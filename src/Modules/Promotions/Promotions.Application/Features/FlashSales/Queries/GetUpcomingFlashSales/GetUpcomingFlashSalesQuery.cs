namespace Promotions.Application.Features.FlashSales.Queries.GetUpcomingFlashSales
{
    public sealed record GetUpcomingFlashSalesQuery : ICachedQuery<IReadOnlyList<FlashSaleDto>>
    {
        public string Key => "UpcomingFlashSales";
        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);
    }
}
