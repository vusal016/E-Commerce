namespace Web.Api.Middleware
{
    internal sealed class GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                int statusCode = ex switch
                {
                    UnauthorizedAccessException => 401,
                    ForbiddenException => 403,
                    ArgumentException => 400,
                    KeyNotFoundException => 404,
                    InvalidOperationException => 409,
                    DbUpdateConcurrencyException => 409,
                    DbUpdateException dbEx when IsUniqueViolation(dbEx) => 409,
                    _ => 500
                };
                if (statusCode == 500)
                {
                    logger.LogError(ex, "An unexpected error occurred.");
                }
                else
                {
                    logger.LogWarning(ex, "Request failed with status code {StatusCode}.", statusCode);
                }
                string message = statusCode == 500
                    ? "An unexpected error occurred."
                    : statusCode == 409 && ex is DbUpdateConcurrencyException or DbUpdateException
                        ? "Concurrency or duplicate resource conflict."
                        : ex.Message;
                Result<string> response = Result<string>.Fail(
                    message,
                    statusCode);
                context.Response.StatusCode = statusCode;
                await context.Response.WriteAsJsonAsync(response);
            }
        }
        private static bool IsUniqueViolation(DbUpdateException ex)
        {
            var inner = ex.InnerException;
            var sqlStateProp = inner?.GetType().GetProperty("SqlState");
            return sqlStateProp?.GetValue(inner) as string == "23505";
        }
    }
}