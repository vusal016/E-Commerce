namespace Catalog.Application.Common.Dtos
{
    public sealed record SearchResponseDto(
        IReadOnlyList<SearchProductDto> Products,
        string? SuggestedQuery);
}