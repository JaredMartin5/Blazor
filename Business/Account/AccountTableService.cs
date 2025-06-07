using Data;
using Microsoft.EntityFrameworkCore;
using OneOf;
using Shared.AccountUser;
using Shared.Common;

namespace Business.Account;

public class AccountTableService : IAccountTableService
{
    private readonly ApplicationDbContext _dbContext;

    public AccountTableService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<OneOf<List<AccountUserDto>, ApiErrorResult>> GetAllAccounts()
    {
        try
        {
            var users = await _dbContext.Users.Select(u => new AccountUserDto(u.Email ?? "")).ToListAsync();

            return users;
        }
        catch (Exception ex)
        {
            return new ApiErrorResult([new("Users", $"Failed to retrieve users: {ex.Message}")]);
        }
    }
}
