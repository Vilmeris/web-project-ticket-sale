using System;
using System.Collections.Generic; // <-- BU SATIR EKLENDİ (ICollection için şart)
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Programming_Project.Models
{
    [Table("Events")]
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [Display(Name = "Etkinlik Başlığı")]
        [Required(ErrorMessage = "Başlık zorunludur.")]
        public string Title { get; set; }

        [Display(Name = "Sanatçı / Grup / Takım")]
        [Required(ErrorMessage = "Lütfen sahne alacak kişi/kurumu giriniz.")]
        public string Artist { get; set; }

        [Display(Name = "Açıklama")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Kategori")]
        [Required]
        public string Category { get; set; }

        [Display(Name = "Görsel URL")]
        public string ImageUrl { get; set; }

        [Display(Name = "Tarih ve Saat")]
        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "Şehir")]
        [Required]
        public string City { get; set; }

        [Display(Name = "Mekan / Salon")]
        [Required]
        public string Venue { get; set; }

        [Display(Name = "Toplam Kapasite")]
        [Required]
        public int Capacity { get; set; }

        [Display(Name = "Satılan Bilet")]
        public int SoldTicketCount { get; set; } = 0;

        // Bu alan hem Sinema için tek fiyatı tutacak, 
        // hem de Tiyatro/Konser için "En düşük başlangıç fiyatını" tutacak (Vitrin için).
        [Display(Name = "Bilet Fiyatı")]
        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public string SubCategory { get; set; }

        [Display(Name = "İndirim Oranı (%)")]
        [Range(0, 100, ErrorMessage = "0 ile 100 arasında olmalı")]
        public int DiscountRate { get; set; } = 0;

        [NotMapped]
        public decimal DiscountedPrice
        {
            get { return Price - (Price * DiscountRate / 100); }
        }

        // --- KRİTİK EKLEME: FİYAT BÖLGELERİ İLİŞKİSİ ---
        // Bu satır sayesinde EventPrice tablosuna ulaşabileceğiz.
        public virtual ICollection<EventPrice> EventPrices { get; set; }
    }
}