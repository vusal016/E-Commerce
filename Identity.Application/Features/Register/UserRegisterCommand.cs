namespace Identity.Application.Features.Register
{
    public record UserRegisterCommand(string FirstName, string LastName, string Email, string Password) : IRequest<AuthResponseDto>;
}