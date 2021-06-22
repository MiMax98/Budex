using System.ComponentModel.DataAnnotations;

namespace BuildingMaterialRent.Models
{
    public class RegisterViewModel
    {
        [Display(Name = "Imię")]
        [Required(ErrorMessage = "{0} jest wymagane")]
        public string FirstName { get; set; }
        [Display(Name = "Nazwisko")]
        [Required(ErrorMessage = "{0} jest wymagane")]
        public string LastName { get; set; }
        [EmailAddress(ErrorMessage = "Nieprawidłowy format e-mail")]
        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "{0} jest wymagany")]
        public string Email { get; set; }

        [Required(ErrorMessage ="{0} jest wymagane")]
        [StringLength(100, ErrorMessage = "{0} musi mieć długoś przynajmniej {2} znaków", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdzenie hasła")]
        [Compare("Password", ErrorMessage = "Hasło musi się zgadzać z potwierdzeniem")]
        public string ConfirmPassword { get; set; }
    }
}
