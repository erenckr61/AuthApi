using Microsoft.AspNetCore.Identity;

namespace AuthApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; } 
    }
}
//kullanıcı bilgilerini temsil  eder