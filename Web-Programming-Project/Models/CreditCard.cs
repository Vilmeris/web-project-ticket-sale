using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Programming_Project.Models
{
    public class CreditCard
    {
        [Key]
        public int CardId { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Required(ErrorMessage = "Kart numarası gereklidir.")]
        [Display(Name = "Kart Numarası")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "16 haneli numara giriniz.")]
        public string CardNumber { get; set; }

        [Required]
        [Display(Name = "Kart Sahibi")]
        public string CardHolderName { get; set; }

        [Required]
        [Display(Name = "Son Kullanma (AA/YY)")]
        public string ExpiryDate { get; set; }

        [Required]
        [Display(Name = "CVV")]
        public string CVV { get; set; }

        public decimal CardLimit { get; set; } = 1000000;
    }
}