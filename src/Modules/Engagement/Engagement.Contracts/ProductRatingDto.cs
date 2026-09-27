namespace Engagement.Contracts;

public sealed record ProductRatingDto(
    Guid ProductId,
    decimal RatingAverage,
    int ReviewCount);
