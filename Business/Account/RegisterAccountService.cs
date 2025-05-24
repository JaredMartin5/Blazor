using Business.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using OneOf;
using Shared.Common;
using Shared.Register;

namespace Business.Account;

public class RegisterAccountService : IRegisterAccountService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IValidator<RegisterModel> _validator;

    public RegisterAccountService(UserManager<IdentityUser> userManager, IValidator<RegisterModel> validator)
    {
        _userManager = userManager;
        _validator = validator;
    }

    public async Task<OneOf<bool, ApiErrorResult>> RegisterAccount(RegisterModel registerModel)
    {
        var validationResult = await _validator.ValidateAsync(registerModel);
        if (validationResult.Errors.Count != 0)
            return validationResult.GetApiErrorResult();

        var newUser = new IdentityUser { UserName = registerModel.Email, Email = registerModel.Email };
        var result = await _userManager.CreateAsync(newUser, registerModel.Password!);

        if (!result.Succeeded)
            return new ApiErrorResult(result.Errors.Select(e => new ApiError(nameof(RegisterModel.Email), e.Description)));

        return true;
    }
}
