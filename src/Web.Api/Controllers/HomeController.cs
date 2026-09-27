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

        [HttpGet("curated-picks")]
        public async Task<IActionResult> GetCuratedPicks(CancellationToken cancellationToken)
        {
            var request = await mediator.Send(new GetCuratedPicksQuery(), cancellationToken);
            var response = Result<IReadOnlyList<CuratedPickDto>>.Success(request, StatusCodes.Status200OK);
            return Ok(response);
        }
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
        {
            var request = await mediator.Send(new GetHomeCategoriesQuery(), cancellationToken);
            var response = Result<IReadOnlyList<HomeCategoryDto>>.Success(request, StatusCodes.Status200OK);
            return Ok(response);
        }
    }
}
