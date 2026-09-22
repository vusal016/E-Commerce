namespace SharedKernel.Cache
{
    public interface ICachedQuery
    {
        public string Key { get; }
        TimeSpan? Expiration { get; }
    }
    public interface ICachedQuery<TResponse> :IRequest<TResponse>, ICachedQuery
    {
    }
}
