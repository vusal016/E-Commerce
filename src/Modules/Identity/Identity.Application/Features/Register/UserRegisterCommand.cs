namespace Identity.Application.Features.Register
{
    internal sealed record UserRegisterCommand(string FirstName, string LastName, string Email, string Password) : IRequest<AuthResponseDto>;
}