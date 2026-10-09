namespace Engagement.Domain.SupportAggregate
{
    public sealed class SupportArticle : AuditEntity
    {
        private SupportArticle()
        {
        }
        public SupportArticle(string title, string content, string category)
        {
            SetTitle(title);
            SetContent(content);
            SetCategory(category);
        }
        public string Title { get; private set; }
        public string Content { get; private set; }
        public string Category { get; private set; }
        private void SetTitle(string title)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(title, "Title cannot be empty.");
            Title = title;
        }
        private void SetContent(string content)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(content, "Content cannot be empty.");
            Content = content;
        }
        private void SetCategory(string category)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(category, "Category cannot be empty.");
            Category = category;
        }
    }
}