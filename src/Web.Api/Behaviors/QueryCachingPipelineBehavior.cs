namespace Web.Api.Behaviors
{
    internal sealed class QueryCachingPipelineBehavior<TRequest, TResponse>(ICacheService cacheService) : IPipelineBehavior<TRequest, TResponse> where TRequest : ICachedQuery
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            return await cacheService.GetOrSetAsync(
             request.Key,
             _ => next(),
             request.Expiration,
             cancellationToken
            );
        }
    }
}