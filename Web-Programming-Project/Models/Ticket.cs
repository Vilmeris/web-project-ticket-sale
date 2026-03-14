using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Programming_Project.Models
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }

        [Required]
        public int EventId { get; set; }
        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Required]
        [Display(Name = "Koltuk No")]
        public string SeatNumber { get; set; }

        [Display(Name = "Satın Alma Tarihi")]
        public DateTime PurchaseDate { get; set; }

        public decimal PricePaid { get; set; }


    }
}