namespace Cart.Application.Features.CartActions;

public sealed record ApplyCouponCommand(Guid? UserId, string? SessionId, string Code) : IRequest<CartDto>;



