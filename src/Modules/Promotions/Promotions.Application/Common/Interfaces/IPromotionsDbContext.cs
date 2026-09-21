namespace Promotions.Application.Common.Interfaces
{
    public interface IPromotionsDbContext
    {
        DbSet<HeroBanner> HeroBanners { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}

