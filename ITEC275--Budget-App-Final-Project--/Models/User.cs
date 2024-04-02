using Microsoft.AspNetCore.Identity;

namespace ITEC275__Budget_App_Final_Project__.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
