namespace Identity.Application.Features.Register
{
    public sealed class UserRegisterCommandHandler(UserManager<User> userManager,IIdentityDbContext identityDbContext,ITokenProvider tokenProvider,IMapper mapper) : IRequestHandler<UserRegisterCommand, AuthResponseDto>
    {
        public async Task<AuthResponseDto> Handle(UserRegisterCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await userManager.FindByEmailAsync(request.Email);
            if (existingUser is not null) throw new InvalidOperationException("User with this email already exists.");
            var user = new User
                (
                      request.FirstName,
                      request.LastName,
                      request.Email
                );
            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Invalid email or password. Details: {errors}");
            }
            var userDto=mapper.Map<UserDto>(user);
            var accessToken = tokenProvider.GenerateAccessToken(user);
            var refreshToken = tokenProvider.GenerateRefreshToken();
            var refreshTokenEntity = new RefreshToken
            (
                user.Id,
                refreshToken,
                tokenProvider.RefreshTokenExpiresAt
            );
            await identityDbContext.RefreshTokens.AddAsync(refreshTokenEntity, cancellationToken);
            await identityDbContext.SaveChangesAsync(cancellationToken);
            return new AuthResponseDto
                (
                    userDto,
                    accessToken,
                    refreshToken
                );
        }
    }
}


// Create User ve Refresh token elavesi ayri transaction daxilinde calisir(refactor xatirlatmasi)