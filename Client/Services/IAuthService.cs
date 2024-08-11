using Shared.Common;
using Shared.Login;
using Shared.Register;

namespace Client.Services;

public interface IAuthService
{
    Task<ApiResult<string>> Login(LoginModel loginModel);
    Task Logout();
    Task<ApiResult<object>> Register(RegisterModel registerModel);
}