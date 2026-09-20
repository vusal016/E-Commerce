namespace Identity.Application.Features.Login
{
    public sealed class UserLoginCommandHandler(UserManager<User> userManager,IIdentityDbContext identityDbContext,ITokenProvider tokenProvider,IMapper mapper) : IRequestHandler<UserLoginCommand, AuthResponseDto>
    {
        public async Task<AuthResponseDto> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email)
            ?? throw new UnauthorizedAccessException("Unauthorized access");
            if(await userManager.IsLockedOutAsync(user)) throw new UnauthorizedAccessException("Unauthorized access");
            var isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                await userManager.AccessFailedAsync(user);
                throw new UnauthorizedAccessException("Invalid password or email");
            }
            await userManager.ResetAccessFailedCountAsync(user);
            await identityDbContext.RefreshTokens.Where(x => x.UserId == user.Id).ExecuteDeleteAsync(cancellationToken);
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
            var userDto = mapper.Map<UserDto>(user);

            return new AuthResponseDto
            (
                userDto,
                accessToken,
                refreshToken
            );
        }
    }
}