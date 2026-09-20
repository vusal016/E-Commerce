namespace Identity.Application.Common.IdentityMapper
{
    public sealed class IdentityMapper :Profile
    {
        public IdentityMapper()
        {
            CreateMap<User, UserDto>()
            .ForCtorParam(nameof(UserDto.Phone), opt => opt.MapFrom(src => src.PhoneNumber));
        }
    }
}