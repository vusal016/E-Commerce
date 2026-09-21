namespace Promotions.Domain.HeroBannerAggregate
{
    public sealed class HeroBanner : AuditEntity
    {
        private HeroBanner()
        {
        }

        public HeroBanner(string title, string? subtitle, string imageUrl, string? mobileImageUrl, string linkUrl, string? buttonText, int displayOrder, bool isActive)
        {
            SetTitle(title);
            SetImageUrl(imageUrl);
            SetLinkUrl(linkUrl);
            SetDisplayOrder(displayOrder);
            Subtitle = subtitle;
            MobileImageUrl = mobileImageUrl;
            ButtonText = buttonText;
            IsActive = isActive;
        }

        public string Title { get; private set; }
        public string? Subtitle { get; private set; }
        public string ImageUrl { get; private set; }
        public string? MobileImageUrl { get; private set; }
        public string LinkUrl { get; private set; }
        public string? ButtonText { get; private set; }
        public int DisplayOrder { get; private set; }
        public bool IsActive { get; private set; }

        private void SetTitle(string title)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace("Hero banner title cannot be empty.");
            Title = title;
        }

        private void SetImageUrl(string imageUrl)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace("Hero banner image URL cannot be empty.");
            ImageUrl = imageUrl;
        }

        private void SetLinkUrl(string linkUrl)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace("Hero banner link URL cannot be empty.");
            LinkUrl = linkUrl;
        }

        private void SetDisplayOrder(int displayOrder)
        {
            if (displayOrder < 0)
                throw new ArgumentException("Hero banner display order cannot be negative.");
            DisplayOrder = displayOrder;
        }
    }
}

