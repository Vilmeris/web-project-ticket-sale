using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Programming_Project.Models
{
    [Table("EventPrices")]
    public class EventPrice
    {
        [Key]
        public int Id { get; set; }

        public int EventId { get; set; } 

        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }

        [Required]
        [Display(Name = "Bölge Adı (Örn: VIP, Sahne Önü)")]
        public string ZoneName { get; set; }

        [Required]
        [Display(Name = "Fiyat")]
        public decimal Price { get; set; }

        public virtual ICollection<EventPrice> EventPrices { get; set; }

        [Display(Name = "Kontenjan")]
        public int Capacity { get; set; } 
    }
}