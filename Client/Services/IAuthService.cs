using OneOf;
using Shared.Common;
using Shared.Login;
using Shared.Register;

namespace Client.Services;

public interface IAuthService
{
    Task<OneOf<string, ApiErrorResult>> Login(LoginModel loginModel);
    Task Logout();
    Task<OneOf<bool, ApiErrorResult>> Register(RegisterModel registerModel);
}
