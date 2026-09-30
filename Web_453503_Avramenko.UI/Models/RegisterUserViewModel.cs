using System.ComponentModel.DataAnnotations;

namespace Web_453503_Avramenko.UI.Models;

public class RegisterUserViewModel
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
    public IFormFile? Avatar { get; set; }
}