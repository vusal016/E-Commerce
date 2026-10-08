namespace Catalog.Application.Common.Dtos
{
    public sealed record GetProductByIdDto(
        Guid Id,
        string Name,
        string Description,
        decimal BasePrice,
        decimal RatingAverage,
        int ReviewCount,
        List<ProductImageDto> Images,
        List<ProductVariantDto> Variants,
        List<RelatedProductDto> RelatedProducts);
}