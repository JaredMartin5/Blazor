using FluentValidation;

namespace Shared.Register;

public class RegisterModelValidator : AbstractValidator<RegisterModel>
{
    public RegisterModelValidator()
    {
        RuleFor(x => x.Email).Cascade(CascadeMode.Stop).NotEmpty().EmailAddress();

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

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .WithMessage("Confirmation password is required.")
            .Equal(x => x.Password)
            .WithMessage("The password and confirmation password do not match.");
    }
}
