using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Shared.Common;
using Shared.Register;

namespace Api.Account;

public class CreateAccountCommand : IRequest<ApiResult>
{
    public RegisterModel RegisterModel { get; set; } = new();
}

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, ApiResult>
{
    private readonly UserManager<IdentityUser> _userManager;

    public CreateAccountCommandHandler(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ApiResult> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var newUser = new IdentityUser { UserName = request.RegisterModel.Email, Email = request.RegisterModel.Email };

        var result = await _userManager.CreateAsync(newUser, request.RegisterModel.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => new Error(nameof(RegisterModel.Email), e.Description)).ToList();

            return new ApiResult { Errors = errors };
        }

        return new ApiResult();
    }
}