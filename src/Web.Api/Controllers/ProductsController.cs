namespace Web.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery(Name = "category")] string? category = null, string? sort = null, string? size = null, string? color = null, decimal? minPrice = null, decimal? maxPrice = null, int page = 1, int limit = 10, CancellationToken cancellationToken = default)
    {
        var query = new GetProductsWithFiltersQuery(sort, category, size, color, minPrice, maxPrice, page, limit);
        var (products, pagination) = await mediator.Send(query, cancellationToken);
        var response = Result<IReadOnlyList<GetProductsWithFiltersDto>>.Success(products, StatusCodes.Status200OK);
        response.SetPaginationInfo(pagination);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(Guid id, CancellationToken cancellationToken = default)
    {
        var request = await mediator.Send(new GetProductByIdQuery(id), cancellationToken);
        var response = Result<GetProductByIdDto>.Success(request, StatusCodes.Status200OK);
        return Ok(response);
    }
}
