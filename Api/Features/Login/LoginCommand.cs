using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Shared.Common;
using Shared.Login;

namespace Api.Features.Login;

public class LoginCommand : IRequest<ApiResult>
{
    public LoginModel LoginModel { get; set; } = new();
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, ApiResult>
{
    private readonly IConfiguration _configuration;
    private readonly SignInManager<IdentityUser> _signInManager;

    public LoginCommandHandler(IConfiguration configuration, SignInManager<IdentityUser> signInManager)
    {
        _configuration = configuration;
        _signInManager = signInManager;
    }

    public async Task<ApiResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await _signInManager.PasswordSignInAsync(request.LoginModel.Email, request.LoginModel.Password, false, false);

        if (!result.Succeeded)
            return new ApiResult
            {
                Successful = false,
                Errors = new List<Error> { new(nameof(LoginModel.Email), "Username and password are invalid.") },
            };

        var claims = new[] { new Claim(ClaimTypes.Name, request.LoginModel.Email) };

        var keyString = _configuration["JwtSecurityKey"] ?? string.Empty;
        if (keyString.Length < 32)
            return new ApiResult
            {
                Successful = false,
                Errors = new List<Error> { new(nameof(LoginModel.Email), "Security key is too short. It must be at least 32 characters long.") },
            };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiry = DateTime.Now.AddDays(Convert.ToInt32(_configuration["JwtExpiryInDays"]));

        var token = new JwtSecurityToken(_configuration["JwtIssuer"], _configuration["JwtAudience"], claims, expires: expiry, signingCredentials: creds);

        return new ApiResult { Successful = true, Data = new JwtSecurityTokenHandler().WriteToken(token) };
    }
}
