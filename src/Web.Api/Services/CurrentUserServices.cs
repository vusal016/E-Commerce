namespace Web.Api.Services
{
    internal sealed class CurrentUserServices(IHttpContextAccessor accessor) : ICurrentUserService
    {
        public Guid? UserId
        {
            get
            {
                var context = accessor.HttpContext;
                if (context?.User.Identity?.IsAuthenticated != true)
                    return null;
                var idClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                return Guid.TryParse(idClaim, out var userId) && userId != Guid.Empty ? userId : null;
            }
        }
        public Guid RequiredUserId => UserId ?? throw new UnauthorizedAccessException("User is not authenticated.");
        public string? SessionId
        {
            get
            {
                var context = accessor.HttpContext;
                var raw = context?.Request.Headers["x-session-id"].FirstOrDefault()?.Trim();
                return !string.IsNullOrWhiteSpace(raw) && raw.Length <= 128 ? raw : null;
            }
        }
    }
}