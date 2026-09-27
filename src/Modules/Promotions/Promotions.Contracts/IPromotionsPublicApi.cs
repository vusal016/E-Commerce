namespace Promotions.Contracts;

public interface IPromotionsPublicApi
{
    Task<List<ActiveDiscountDto>> GetActiveDiscountsAsync(IEnumerable<Guid> variantIds, CancellationToken cancellationToken = default);
    Task<CouponDto?> GetCouponAsync(string code, CancellationToken cancellationToken = default);
}

