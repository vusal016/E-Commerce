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
    }
}