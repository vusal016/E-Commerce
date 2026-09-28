namespace Cart.Application.Features.ApplyCoupon;

public sealed record ApplyCouponCommand(Guid? UserId, string? SessionId, string Code) : IRequest<CartDto>;




