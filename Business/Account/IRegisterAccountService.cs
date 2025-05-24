using OneOf;
using Shared.Common;
using Shared.Register;

namespace Business.Account;

public interface IRegisterAccountService
{
    Task<OneOf<bool, ApiErrorResult>> RegisterAccount(RegisterModel registerModel);
}
