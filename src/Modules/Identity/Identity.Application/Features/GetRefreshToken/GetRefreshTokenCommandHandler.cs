namespace Identity.Application.Features.GetRefreshToken
{
    public sealed class GetRefreshTokenCommandHandler(UserManager<User> userManager, IIdentityDbContext identityDbContext, ITokenProvider tokenProvider) : IRequestHandler<GetRefreshTokenCommand, RefreshTokenDto>
    {
        public async Task<RefreshTokenDto> Handle(GetRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(request.RefreshToken);
            var refToken = await identityDbContext.RefreshTokens.AsNoTracking()
           .FirstOrDefaultAsync(x => x.Token == request.RefreshToken, cancellationToken)
           ?? throw new UnauthorizedAccessException("Invalid refresh token.");

            var user = await userManager.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == refToken.UserId, cancellationToken)
            ?? throw new UnauthorizedAccessException("User not found.");

            await identityDbContext.RefreshTokens.Where(r => r.Id == refToken.Id).ExecuteDeleteAsync(cancellationToken);

            var accessToken = tokenProvider.GenerateAccessToken(user);
            var refreshToken = tokenProvider.GenerateRefreshToken();

            var newRefreshToken = new RefreshToken
            (
                user.Id,
                refreshToken,
                tokenProvider.RefreshTokenExpiresAt
            );

            await identityDbContext.RefreshTokens.AddAsync(newRefreshToken, cancellationToken);
            await identityDbContext.SaveChangesAsync(cancellationToken);

            return new RefreshTokenDto
            (
                accessToken,
                newRefreshToken.Token
            );  
        }
    }
}