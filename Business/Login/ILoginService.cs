using OneOf;
using Shared.Common;
using Shared.Login;

namespace Business.Login;

public interface ILoginService
{
    Task<OneOf<string, ApiErrorResult>> LoginAsync(LoginModel model);
}
