namespace Identity.Application.Common.Dtos
{
    internal sealed record AuthResponseDto
    (
        UserDto User,
        string AccessToken,
        string RefreshToken
    );
}