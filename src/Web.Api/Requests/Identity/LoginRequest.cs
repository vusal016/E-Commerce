namespace Web.Api.Requests.Identity
{
    public sealed record LoginRequest(string Email, string Password);
}