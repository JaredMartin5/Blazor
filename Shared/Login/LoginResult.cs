namespace Shared.Login;

public class LoginResult
{
    public bool Successful { get; set; }
    public string Error { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}

/*public class LoginResult
{
    public bool Successful { get; set; }
    public Error Error { get; set; } = new(string.Empty, string.Empty);
    public string Token { get; set; } = string.Empty;
}*/