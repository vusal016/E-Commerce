namespace Identity.Application.Common.Interfaces
{
    public interface IIdentityDbContext
    {
        DbSet<User> Users { set; get; }
        DbSet<RefreshToken> RefreshTokens { set; get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}