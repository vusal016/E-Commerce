namespace Web.Api.Services
{
    public sealed class CurrentUserServices(IHttpContextAccessor accessor) : ICurrentUserService
    {
        public Guid? UserId
        {
            get
            {
                var context = accessor.HttpContext;
                if (context?.User.Identity?.IsAuthenticated != true)
                    return null;

                var idClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                return idClaim is not null ? Guid.Parse(idClaim) : null;
            }
        }

        public string? SessionId
        {
            get
            {
                var context = accessor.HttpContext;
                return context?.Request.Headers["x-session-id"].FirstOrDefault();
            }
        }
    }
}