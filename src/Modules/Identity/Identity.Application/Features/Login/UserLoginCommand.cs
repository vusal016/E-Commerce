namespace Identity.Application.Features.Login
{
    internal sealed record UserLoginCommand(string Email, string Password) : IRequest<AuthResponseDto>;
}