using System.ComponentModel.DataAnnotations;

namespace AuthApi.Models
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "İsim gereklidir.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyisim gereklidir.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "E-posta adresi gereklidir.")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Şifre  gereklidir.")]
        public string Password { get; set; }
    }
}
