using Shared.Common;
using Shared.Login;
using Shared.Register;

namespace Client.Services;

public interface IAuthService
{
    Task<LoginResult> Login(LoginModel loginModel);
    Task Logout();
    Task<ApiResult> Register(RegisterModel registerModel);
}