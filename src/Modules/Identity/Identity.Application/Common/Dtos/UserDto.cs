namespace Identity.Application.Common.Dtos
{
    internal sealed record UserDto
    (
        Guid Id,
       string FirstName,
        string LastName,
        string Email,
        string? Phone,
        MemberTier MemberTier,
        string? AvatarUrl,
        bool IsGuest,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );
}