using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Business.Extensions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OneOf;
using Shared.Common;
using Shared.Login;

namespace Business.Login;

public class LoginService : ILoginService
{
    private readonly IConfiguration _configuration;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IValidator<LoginModel> _validator;

    public LoginService(IConfiguration configuration, SignInManager<IdentityUser> signInManager, IValidator<LoginModel> validator)
    {
        _configuration = configuration;
        _signInManager = signInManager;
        _validator = validator;
    }

    public async Task<OneOf<string, ApiErrorResult>> LoginAsync(LoginModel model)
    {
        var validationResult = await _validator.ValidateAsync(model);
        if (validationResult.Errors.Count != 0)
            return validationResult.GetApiErrorResult();

        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
        if (!result.Succeeded)
            return new ApiErrorResult([new(nameof(LoginModel.Email), "Username and password are invalid.")]);

        var claims = new[] { new Claim(ClaimTypes.Name, model.Email!) };

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
