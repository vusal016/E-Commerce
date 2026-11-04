namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IMediator mediator) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
        {
            var command = new UserRegisterCommand(request.FirstName, request.LastName, request.Email, request.Password);
            var result = await mediator.Send(command, cancellationToken);
            var response = Result<AuthResponseDto>.Success(result, StatusCodes.Status200OK);
            return Ok(response);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            var command = new UserLoginCommand(request.Email, request.Password);
            var result = await mediator.Send(command, cancellationToken);
            var response = Result<AuthResponseDto>.Success(result, StatusCodes.Status200OK);
            return Ok(response);
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var command = new GetRefreshTokenCommand(request.RefreshToken);
            var result = await mediator.Send(command, cancellationToken);
            var response = Result<RefreshTokenDto>.Success(result, StatusCodes.Status200OK);
            return Ok(response);
        }
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMe(CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var request = await mediator.Send(new GetMeQuery(userId), cancellationToken);
            var response = Result<UserDto>.Success(request, StatusCodes.Status200OK);
            return Ok(response);
        }
    }
}