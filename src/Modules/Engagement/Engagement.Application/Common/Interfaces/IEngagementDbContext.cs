namespace Engagement.Application.Common.Interfaces
{
    public interface IEngagementDbContext
    {
        DbSet<Review> Reviews { get; set; }
        DbSet<Faq> Faqs { get; set; }
        DbSet<SupportArticle> SupportArticles { get; set; }
        DbSet<Notification> Notifications { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}

