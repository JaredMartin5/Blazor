using Api.Extensions;
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

    public async Task<OneOf<bool, ApiErrorResult>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request.RegisterModel, cancellationToken);
        if (validationResult.Errors.Count != 0)
            return validationResult.GetApiErrorResult();

        var newUser = new IdentityUser { UserName = request.RegisterModel.Email, Email = request.RegisterModel.Email, };
        var result = await _userManager.CreateAsync(newUser, request.RegisterModel.Password);

        if (!result.Succeeded)
            return new ApiErrorResult(result.Errors.Select(e => new ApiError(nameof(RegisterModel.Email), e.Description)).ToList());

        return true;
    }
}
