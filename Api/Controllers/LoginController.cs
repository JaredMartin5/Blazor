using Api.Features.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Login;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly IMediator _mediator;

    public LoginController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
    {
        var result = await _mediator.Send(new LoginCommand { LoginModel = loginModel });

        return result.Successful ? Ok(result) : BadRequest(result);
    }
}
