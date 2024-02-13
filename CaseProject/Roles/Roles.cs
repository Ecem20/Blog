using Microsoft.AspNetCore.Mvc.Rendering;

namespace CaseProject.Roles
{
    public class Roles
    {
        public static string admin = "admin";
        public static string blogger = "blogger";

        public static List<SelectListItem> GetRoles()
        {
            return new List<SelectListItem>
            {
                new SelectListItem{Value=Roles.admin,Text=Roles.admin},
                new SelectListItem{Value=Roles.blogger,Text=Roles.blogger},
            };
        }
    }
}
