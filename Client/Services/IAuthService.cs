using OneOf;
using Shared.Common;
using Shared.Login;
using Shared.Register;

namespace Client.Services;

public interface IAuthService
{
    Task<OneOf<string, List<ApiError>>> Login(LoginModel loginModel);
    Task Logout();
    Task<OneOf<bool, List<ApiError>>> Register(RegisterModel registerModel);
}
