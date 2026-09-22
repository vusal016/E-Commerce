namespace Web.Api.Caching
{
    public sealed class CacheService(IDistributedCache cache) : ICacheService
    {
        public async Task<T> GetOrSetAsync<T>(string key, Func<CancellationToken, Task<T>> factory, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
           var cacheedData= await cache.GetStringAsync(key, cancellationToken);
            if (cacheedData is not null)
            {
                return JsonSerializer.Deserialize<T>(cacheedData)!;
            }
            var data = await factory(cancellationToken);

            await cache.SetStringAsync(
               key,
               JsonSerializer.Serialize(data),
               new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(2) },
               cancellationToken);

            return data;
        }
    }
}