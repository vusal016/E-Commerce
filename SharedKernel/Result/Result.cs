namespace SharedKernel.Result
{
    public sealed class Result<T>
    {
        private Result()
        {

        }
        public bool IsSuccess { get; private set; }
        public int StatusCode { get; private set; }
        public IReadOnlyList<string> Errors { get; private set; }
        public T Data { get; private set; }

        public static Result<T> Success(T data, int statusCode)
        {
            return new Result<T>()
            {
                IsSuccess = true,
                Data = data,
                StatusCode = statusCode,
            };
        }
        public static Result<T> Success(int statusCode)
        {
            return new Result<T>()
            {
                IsSuccess = true,
                StatusCode = statusCode
            };
        }
        public static Result<T> Fail(IEnumerable<string> errors, int statusCode)
        {
            return new Result<T>()
            {
                IsSuccess = false,
                Errors = new List<string>(errors),
                StatusCode = statusCode
            };
        }
        public static Result<T> Fail(string error, int statusCode)
        {
            return new Result<T>()
            {
                IsSuccess = false,
                Errors = new List<string> { error },
                StatusCode = statusCode
            };
        }
    }
}