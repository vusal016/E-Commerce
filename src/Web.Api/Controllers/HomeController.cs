namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController(IMediator mediator) : ControllerBase
    {
        [HttpGet("hero-banners")]
        public async Task<IActionResult> GetHeroBanners(CancellationToken cancellationToken)
        {
            var request = await mediator.Send(new GetHeroBannersQuery(), cancellationToken);
            var response = Result<IReadOnlyList<HeroBannerDto>>.Success(request, StatusCodes.Status200OK);
            return Ok(response);
        }
    }
}

