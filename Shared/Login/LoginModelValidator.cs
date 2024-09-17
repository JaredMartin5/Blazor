using FluentValidation;

namespace Shared.Login;

public class LoginModelValidator : AbstractValidator<LoginModel>
{
    public LoginModelValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters long.")
            .Matches(@"[A-Z]")
            .WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]")
            .WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"[0-9]")
            .WithMessage("Password must contain at least one number.")
            .Matches(@"[\!\?\*\.]")
            .WithMessage("Password must contain at least one special character (e.g. !, ?, *, .)");

        RuleFor(x => x.Email).Cascade(CascadeMode.Stop).NotEmpty().EmailAddress();
    }
}
