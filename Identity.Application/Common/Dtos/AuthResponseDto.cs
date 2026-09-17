namespace Identity.Application.Common.Dtos
{
    public record AuthResponseDto
   (
        UserDto User,
        string AccessToken,
        string RefreshToken
    );
}