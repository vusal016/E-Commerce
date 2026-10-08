namespace Catalog.Application.Common.Dtos
{
    public sealed record BrandStoreDto(
        BrandInfoDto Brand,
        IReadOnlyList<BrandStoreProductDto> BestSellers,
        IReadOnlyList<BrandStoreProductDto> Products,
        BrandStoreFiltersDto Filters);
}