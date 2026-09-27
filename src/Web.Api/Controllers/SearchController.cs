namespace Web.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SearchController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Search([FromQuery(Name = "q")] string query, int page = 1, int limit = 10, CancellationToken cancellationToken = default)
    {
        var request = new SearchProductsQuery(query, page, limit);
        var (data, pagination) = await mediator.Send(request, cancellationToken);
        var result = Result<SearchResponseDto>.Success(data, StatusCodes.Status200OK);
        result.SetPaginationInfo(pagination);
        return Ok(result);
    }
}
