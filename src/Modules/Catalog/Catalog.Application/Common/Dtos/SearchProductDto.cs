namespace Catalog.Application.Common.Dtos
{
    public sealed record SearchProductDto(
        Guid Id,
        string Name,
        decimal BasePrice,
        string? PrimaryImageUrl,
        string BrandName);
}
