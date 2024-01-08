using System.ComponentModel.DataAnnotations;

namespace Project.Application.Auth.Models;
public class AuthenticateRequest
{
    [Required]
    public string PersonalN { get; set; }
    [Required]
    public string Password { get; set; }
}
