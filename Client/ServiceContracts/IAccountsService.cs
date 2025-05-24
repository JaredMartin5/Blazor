using OneOf;
using Shared.Common;
using Shared.Register;

namespace Client.ServiceContracts;

public interface IAccountsService
{
    Task Logout();
    Task<OneOf<bool, ApiErrorResult>> Register(RegisterModel registerModel);
}
