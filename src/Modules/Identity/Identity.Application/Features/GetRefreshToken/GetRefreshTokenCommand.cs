namespace Identity.Application.Features.GetRefreshToken
{
    internal sealed record GetRefreshTokenCommand(string RefreshToken) : IRequest<RefreshTokenDto>;
}