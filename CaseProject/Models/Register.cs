using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CaseProject.Models
{
    public class Register
    {
        [Required]
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Surname { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        [StringLength(100,ErrorMessage ="The {0} must be at least 6 characters long",MinimumLength =6)]
        public string? Password {  get; set; }

        [Display(Name="Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="The password and confirmaiton password do not match")]
        public string? ConfirmPassword { get; set; }
    }
}
