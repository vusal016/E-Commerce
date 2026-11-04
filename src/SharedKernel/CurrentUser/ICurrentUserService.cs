namespace SharedKernel.CurrentUser
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
        Guid RequiredUserId { get; }
        string? SessionId { get; }
    }
}