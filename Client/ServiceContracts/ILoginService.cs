using OneOf;
using Shared.Common;
using Shared.Login;

namespace Client.ServiceContracts;

public interface ILoginService
{
    Task<OneOf<string, ApiErrorResult>> Login(LoginModel loginModel);
}
