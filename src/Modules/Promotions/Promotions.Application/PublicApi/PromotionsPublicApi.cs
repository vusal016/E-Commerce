using Promotions.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Promotions.Application.Common.Interfaces;

namespace Promotions.Application.PublicApi;

internal sealed class PromotionsPublicApi(IPromotionsDbContext dbContext) : IPromotionsPublicApi
{
    public async Task<List<ActiveDiscountDto>> GetActiveDiscountsAsync(IEnumerable<Guid> variantIds, CancellationToken cancellationToken = default)
    {
        var ids = variantIds.Distinct().ToList();
        var now = DateTime.UtcNow;

        return await dbContext.FlashSales
            .AsNoTracking()
            .Where(fs => fs.IsActive && fs.StartsAt <= now && fs.EndsAt > now)
            .SelectMany(fs => fs.Items)
            .Where(i => ids.Contains(i.ProductVariantId))
            .Select(i => new ActiveDiscountDto(i.ProductVariantId, i.DiscountPercentage))
            .ToListAsync(cancellationToken);
    }

    public async Task<CouponDto?> GetCouponAsync(string code, CancellationToken cancellationToken = default)
    {
        var coupon = await dbContext.Coupons.AsNoTracking().FirstOrDefaultAsync(c => c.Code == code, cancellationToken);
        if (coupon == null) return null;
        return new CouponDto(coupon.Code, coupon.DiscountType, coupon.DiscountValue, coupon.MinOrderAmount, coupon.ExpiresAt, coupon.IsActive);
    }
}
