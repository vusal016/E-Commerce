namespace Promotions.Application.Common.Interfaces
{
    public interface IPromotionsDbContext
    {
        DbSet<HeroBanner> HeroBanners { get; set; }
        DbSet<FlashSale> FlashSales { get; set; }
        DbSet<FlashSaleItem> FlashSaleItems { get; set; }
        DbSet<Coupon> Coupons { get; set; }
        DbSet<NotifyRequest> NotifyRequests { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}

