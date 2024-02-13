using System.ComponentModel.DataAnnotations;

namespace CaseProject.Models
{
    public class Login
    {
        [EmailAddress]
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(Name ="Remember me?")]
        public bool RememberMe { get; set; }
    }
}
