using System.ComponentModel.DataAnnotations;

namespace Web_Programming_Project.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Ad zorunludur.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Soyad zorunludur.")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Telefon zorunludur.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir mail giriniz.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Şifre tekrarı zorunludur.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor!")]
        public string ConfirmPassword { get; set; }
    }
}