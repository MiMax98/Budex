using System.ComponentModel.DataAnnotations;

namespace BuildingMaterialRent.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "{0} jest wymagany")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy format e-mail")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage ="{0} jest wymagane")]
        [Display(Name = "Hasło")]
        public string Password { get; set; }
    }
}
