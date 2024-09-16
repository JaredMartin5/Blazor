using MediatR;
using Microsoft.AspNetCore.Identity;
using OneOf;
using Shared.Common;
using Shared.Register;

namespace Api.Features.Account;

public class CreateAccountCommand : IRequest<OneOf<bool, ApiErrorResult>>
{
    public RegisterModel RegisterModel { get; set; } = new();
}

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, OneOf<bool, ApiErrorResult>>
{
    private readonly UserManager<IdentityUser> _userManager;

    public CreateAccountCommandHandler(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<OneOf<bool, ApiErrorResult>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var newUser = new IdentityUser { UserName = request.RegisterModel.Email, Email = request.RegisterModel.Email };
        var result = await _userManager.CreateAsync(newUser, request.RegisterModel.Password);

        if (!result.Succeeded)
        {
            return new ApiErrorResult(result.Errors.Select(e => new ApiError(nameof(RegisterModel.Email), e.Description)).ToList());
        }

        return true;
    }
}
