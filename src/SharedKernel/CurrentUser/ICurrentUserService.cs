namespace SharedKernel.CurrentUser
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
        string? SessionId { get; }
    }
}
