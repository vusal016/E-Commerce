namespace Identity.Application.Common.Interfaces
{
    public interface ITokenProvider
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        DateTime RefreshTokenExpiresAt { get; }
        int AccessTokenExpirationInSeconds { get; }
    }
}