namespace Catalog.Application.Common.Dtos
{
    public sealed record BrandStoreFiltersDto(
        decimal MinPrice,
        decimal MaxPrice,
        IReadOnlyList<string> AvailableColors,
        IReadOnlyList<string> AvailableSizes);
}
