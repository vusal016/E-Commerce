namespace Identity.Application.Features.Logout
{
    internal sealed record UserLogoutCommand(string RefreshToken) : IRequest<bool>;
}