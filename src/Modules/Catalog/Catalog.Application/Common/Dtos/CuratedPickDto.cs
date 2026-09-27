namespace Catalog.Application.Common.Dtos
{
    public sealed record CuratedPickDto(
        Guid Id,
        string Name,
        string Description,
        decimal BasePrice);
}
