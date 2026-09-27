namespace Identity.Application.Common.Interfaces
{
    public interface IIdentityDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<RefreshToken> RefreshTokens { get; set; }
        DbSet<PaymentMethod> PaymentMethods { get; set; }
        DbSet<Address> Addresses { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}

