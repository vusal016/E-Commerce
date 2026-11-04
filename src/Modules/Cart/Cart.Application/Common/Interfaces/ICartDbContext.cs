namespace Cart.Application.Common.Interfaces
{
    internal interface ICartDbContext
    {
        DbSet<CartEntity> Carts { get; set; }
        DbSet<CartItem> CartItems { get; set; }
        DbSet<Wishlist> Wishlists { get; set; }
        DbSet<WishlistItem> WishlistItems { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}