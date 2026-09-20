namespace Identity.Application.Features.Logout
{
    public sealed class UserLogoutCommandHandler(IIdentityDbContext identityDbContext) : IRequestHandler<UserLogoutCommand, bool>
    {
        public async Task<bool> Handle(UserLogoutCommand request, CancellationToken cancellationToken)
        {
            await identityDbContext.RefreshTokens.Where(r => r.Token == request.RefreshToken).ExecuteDeleteAsync(cancellationToken);
            return true;
        }
    }
}