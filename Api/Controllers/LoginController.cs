using Business.Login;
using Microsoft.AspNetCore.Mvc;
using Shared.Login;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly ILoginService _loginService;

    public LoginController(ILoginService loginService)
    {
        _loginService = loginService;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var result = await _loginService.LoginAsync(model);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
