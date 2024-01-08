using Project.Domain.Accounts;
using Project.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Project.Domain.Users;
public class User : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }
    [Required]
    public int Age { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    [MaxLength(50)]
    public string UserName { get; set; }
    [Required]
    [MaxLength(200)]
    public string Password { get; set; }
    [Required]
    [MaxLength(11)]
    public string PersonalN { get; set; }
    public Guid UserNumber { get; set; }    
    public virtual Account Account { get; set; }
    public DateTime RegistrationDate { get; set; }
}
