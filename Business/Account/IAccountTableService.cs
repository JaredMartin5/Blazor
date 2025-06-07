using Microsoft.AspNetCore.Identity;
using OneOf;
using Shared.AccountUser;
using Shared.Common;

namespace Business.Account;

public interface IAccountTableService
{
    Task<OneOf<List<AccountUserDto>, ApiErrorResult>> GetAllAccounts();
}
