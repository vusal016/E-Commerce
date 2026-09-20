namespace Identity.Application.Features.Profile.Queries
{
    public record GetMeQuery(Guid UserId) : IRequest<UserDto>;
}
