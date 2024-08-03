namespace Shared;

public class RegisterResult
{
    public bool Successful { get; set; }
    public IEnumerable<Error> Errors { get; set; } = new List<Error>();
}

public class Error
{
    public string Property { get; set; }
    public string Message { get; set; }

    public Error(string property, string message)
    {
        Property = property;
        Message = message;
    }
}