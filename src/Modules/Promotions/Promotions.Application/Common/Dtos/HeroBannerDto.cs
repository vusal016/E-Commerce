namespace Promotions.Application.Common.Dtos
{
    public sealed record HeroBannerDto(
        Guid Id,
        string Title,
        string? Subtitle,
        string ImageUrl,
        string? MobileImageUrl,
        string LinkUrl,
        string? ButtonText,
        int DisplayOrder);
}

