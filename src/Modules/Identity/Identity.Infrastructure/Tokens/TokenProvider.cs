namespace Identity.Infrastructure.Identity
{
    public sealed class TokenProvider(IOptions<JwtSettings> jwtSettings) : ITokenProvider
    {
        private readonly JwtSettings configuration = jwtSettings.Value;
        public DateTime RefreshTokenExpiresAt => DateTime.UtcNow.AddDays(configuration.RefreshTokenExpiresDays);

        public int AccessTokenExpirationInSeconds => configuration.ExpirationInMinutes * 60;

        public string GenerateAccessToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject=new ClaimsIdentity([
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email,user.Email??string.Empty),
                    new Claim(JwtRegisteredClaimNames.Name,user.UserName??string.Empty),
              ]),
                Expires = DateTime.UtcNow.AddMinutes(configuration.ExpirationInMinutes),
                SigningCredentials = credentials,
                Issuer = configuration.Issuer,
                Audience = configuration.Audience
            };

            return new JsonWebTokenHandler().CreateToken(tokenDescriptor);
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
    }
}