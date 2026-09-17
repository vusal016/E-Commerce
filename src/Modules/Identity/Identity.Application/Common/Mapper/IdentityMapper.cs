using AutoMapper;

namespace Identity.Application.Common.IdentityMapper
{
    public sealed class IdentityMapper :Profile
    {
        public IdentityMapper()
        {
            CreateMap<User, UserDto>();
        }
    }
}