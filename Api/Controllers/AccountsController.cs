using Api.Account;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Register;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] RegisterModel registerModel)
    {
        var result = await _mediator.Send(new CreateAccountCommand
        {
            RegisterModel = registerModel
        });

        return result.Successful ? Ok(result) : BadRequest(result);
    }
}