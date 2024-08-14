namespace Shared.Common;

public record ApiResult<T> where T : class?
{
    public required bool Successful { get; init; }

    public bool HasErrors => Errors.Any();
    public IEnumerable<Error> Errors { get; init; } = new List<Error>();
    public T? Data { get; set; }
}

public record ApiResult : ApiResult<object>
{
}