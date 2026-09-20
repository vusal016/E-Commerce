namespace Identity.Application.Common.Dtos
{
    public record UserDto
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