namespace Identity.Application.Features.Profile.Queries
{
    internal sealed record GetMeQuery(Guid UserId) : IRequest<UserDto>;
}