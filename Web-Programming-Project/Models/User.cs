using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web_Programming_Project.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Display(Name = "Ad")]
        [Required(ErrorMessage = "Ad alanı zorunludur!")]
        public string Name { get; set; }

        [Display(Name = "Soyad")]
        [Required(ErrorMessage = "Soyad alanı zorunludur!")]
        public string Surname { get; set; }

        [Display(Name = "Telefon")]
        public string Phone { get; set; }

        [Display(Name = "E-Posta")]
        [Required(ErrorMessage = "E-posta alanı zorunludur!")]
        [EmailAddress(ErrorMessage = "Geçersiz E-posta adresi!")]
        public string Email { get; set; }

        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "Şifre zorunludur!")]
        public string Password { get; set; }

        // Admin/Uye ayrımı için gerekli
        public string Role { get; set; }
    }
}