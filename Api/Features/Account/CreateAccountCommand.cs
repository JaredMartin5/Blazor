using FluentValidation;
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
    private readonly IValidator<RegisterModel> _validator;

    public CreateAccountCommandHandler(UserManager<IdentityUser> userManager, IValidator<RegisterModel> validator)
    {
        _userManager = userManager;
        _validator = validator;
    }

    public async Task<ApiResult> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request.RegisterModel, cancellationToken);

        var newUser = new IdentityUser { UserName = request.RegisterModel.Email, Email = request.RegisterModel.Email, };

        var result = await _userManager.CreateAsync(newUser, request.RegisterModel.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => new Error(nameof(RegisterModel.Email), e.Description)).ToList();

            return new ApiResult { Successful = false, Errors = errors };
        }

        return true;
    }
}
