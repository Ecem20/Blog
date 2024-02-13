using Microsoft.AspNetCore.Identity;

namespace CaseProject.Models
{
    public class ApplicationUser :IdentityUser
    {
        public string Name {  get; set; }
        public string SurName { get; set; }

    }
}
