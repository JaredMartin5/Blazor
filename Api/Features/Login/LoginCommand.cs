using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OneOf;
using Shared.Common;
using Shared.Login;

namespace Api.Features.Login;

public class LoginCommand : IRequest<OneOf<string, ApiErrorResult>>
{
    public LoginModel LoginModel { get; set; } = new();
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, OneOf<string, ApiErrorResult>>
{
    private readonly IConfiguration _configuration;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IValidator<LoginModel> _validator;

    public LoginCommandHandler(IConfiguration configuration, SignInManager<IdentityUser> signInManager, IValidator<LoginModel> validator)
    {
        _configuration = configuration;
        _signInManager = signInManager;
        _validator = validator;
    }

    public async Task<OneOf<string, ApiErrorResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request.LoginModel, cancellationToken);
        var result = await _signInManager.PasswordSignInAsync(request.LoginModel.Email, request.LoginModel.Password, false, false);

        if (!result.Succeeded)
            return new ApiResult
            {
                Successful = false,
                Errors = new List<Error> { new(nameof(LoginModel.Email), "Username and password are invalid."), },
            };

        var claims = new[] { new Claim(ClaimTypes.Name, request.LoginModel.Email) };

        var keyString = _configuration["JwtSecurityKey"] ?? string.Empty;
        if (keyString.Length < 32)
            return new ApiErrorResult([new(nameof(LoginModel.Email), "Security key is too short. It must be at least 32 characters long.")]);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiry = DateTime.Now.AddDays(Convert.ToInt32(_configuration["JwtExpiryInDays"]));
        var token = new JwtSecurityToken(_configuration["JwtIssuer"], _configuration["JwtAudience"], claims, expires: expiry, signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
