namespace Identity.Application.Features.Logout
{
    public record UserLogoutCommand(string RefreshToken) : IRequest<bool>;
}
