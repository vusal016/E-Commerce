namespace Identity.Application.Features.Profile.Queries
{
    public sealed class GetMeQueryHandler(UserManager<User>userManager,IMapper mapper) : IRequestHandler<GetMeQuery, UserDto>
    {
        public async Task<UserDto> Handle(GetMeQuery request, CancellationToken cancellationToken)
        {
           var user= await userManager.FindByIdAsync(request.UserId.ToString())
           ?? throw new UnauthorizedAccessException("Invalid token");
           return mapper.Map<UserDto>(user);
        }
    }
}
