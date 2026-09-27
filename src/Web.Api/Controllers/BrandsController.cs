namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController(IMediator mediator) : ControllerBase
    {
        [HttpGet("{slug}")]
        public async Task<IActionResult> GetBrandStore(string slug, CancellationToken cancellationToken = default)
        {
            var request = new GetBrandStoreBySlugQuery(slug);
            var response = await mediator.Send(request, cancellationToken);
            var result = Result<BrandStoreDto>.Success(response, StatusCodes.Status200OK);
            return Ok(result);
        }
    }
}
