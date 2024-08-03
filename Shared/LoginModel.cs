using System.ComponentModel.DataAnnotations;

namespace Shared;

public class LoginModel
{
    [Required]
    //[StringLength(15, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 6)]
    public string Email { get; set; } = string.Empty;

    [Required] public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}