namespace Identity.Application.Features.GetRefreshToken
{
    public record GetRefreshTokenCommand(string RefreshToken) : IRequest<RefreshTokenDto>;
}
