using Shared.Common;

namespace Shared.Register;

public class RegisterResult
{
    public bool Successful => !Errors.Any();
    public IEnumerable<Error> Errors { get; set; } = new List<Error>();
}

public class ApiResult<T> where T : class?
{
    private bool _successful;

    public bool Successful
    {
        get => _successful;
        set => _successful = value ? value : !Errors.Any();
    }

    public IEnumerable<Error> Errors { get; set; } = new List<Error>();
    public T? Data { get; set; }
}

public class ApiResult : ApiResult<object>
{
}