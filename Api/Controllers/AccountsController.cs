using Business.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Register;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IRegisterAccountService _registerAccountService;
    private readonly IAccountTableService _accountTableService;

    public AccountsController(IRegisterAccountService registerAccountService, IAccountTableService accountTableService)
    {
        _registerAccountService = registerAccountService;
        _accountTableService = accountTableService;
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
    {
        var result = await _registerAccountService.RegisterAccount(registerModel);

        return result.Match<IActionResult>(x => Ok(x), BadRequest);
    }

    [HttpGet("table")]
    [Authorize]
    public async Task<IActionResult> Table()
    {
        var result = await _accountTableService.GetAllAccounts();

        return result.Match<IActionResult>(x => Ok(x), BadRequest);
    }
}
