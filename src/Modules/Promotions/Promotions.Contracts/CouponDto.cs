namespace Promotions.Contracts;

public sealed record CouponDto(string Code, string DiscountType, decimal DiscountValue, decimal MinOrderAmount, System.DateTime ExpiresAt, bool IsActive);
