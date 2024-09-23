using System.ComponentModel.DataAnnotations;

namespace AuthApi.Models 
{
    public class LoginRequest
    {
       [Required]
        public string Email { get; set; }
       
        [Required]
        public string Password { get; set; }
    }

}

