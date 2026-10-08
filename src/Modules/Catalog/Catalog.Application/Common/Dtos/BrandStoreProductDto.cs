namespace Catalog.Application.Common.Dtos
{
    public sealed record BrandStoreProductDto(
        Guid Id,
        string Name,
        decimal BasePrice,
        string? PrimaryImageUrl);
}