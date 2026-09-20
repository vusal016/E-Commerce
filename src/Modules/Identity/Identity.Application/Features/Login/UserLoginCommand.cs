namespace Identity.Application.Features.Login
{
    public record UserLoginCommand(string Email, string Password) : IRequest<AuthResponseDto>;
}