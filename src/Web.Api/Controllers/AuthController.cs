namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IMediator mediator) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterCommand command, CancellationToken cancellationToken)
        {
            var request = await mediator.Send(command, cancellationToken);
            var response=Result<AuthResponseDto>.Success(request, StatusCodes.Status200OK);
            return Ok(response);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginCommand command, CancellationToken cancellationToken)
        {
            var request = await mediator.Send(command, cancellationToken);
            var response=Result<AuthResponseDto>.Success(request, StatusCodes.Status200OK);
            return Ok(response);
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(GetRefreshTokenCommand command, CancellationToken cancellationToken)
        {
            var request = await mediator.Send(command, cancellationToken);
            var response=Result<RefreshTokenDto>.Success(request, StatusCodes.Status200OK);
            return Ok(response);
        }
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMe(CancellationToken cancellationToken)
        {
            var userId=Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var request = await mediator.Send(new GetMeQuery(userId), cancellationToken);
            var response=Result<UserDto>.Success(request, StatusCodes.Status200OK);
            return Ok(response);
        }
    }
}