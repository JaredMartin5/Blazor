using Business.Account;
using Microsoft.AspNetCore.Mvc;
using Shared.Register;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IRegisterAccountService _registerAccountService;

    public AccountsController(IRegisterAccountService registerAccountService)
    {
        _registerAccountService = registerAccountService;
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
    {
        var result = await _registerAccountService.RegisterAccount(registerModel);

        return result.Match<IActionResult>(x => Ok(x), BadRequest);
    }
}
