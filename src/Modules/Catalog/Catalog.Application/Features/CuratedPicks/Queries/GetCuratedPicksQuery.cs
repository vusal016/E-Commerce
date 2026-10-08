namespace Catalog.Application.Features.CuratedPicks.Queries
{
    public sealed record GetCuratedPicksQuery() : ICachedQuery<List<CuratedPickDto>>
    {
        public string Key => "curated-picks";
        public TimeSpan? Expiration => TimeSpan.FromMinutes(2);
    }
}