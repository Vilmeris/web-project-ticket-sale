using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Programming_Project.Models
{
    public class CartItem
    {
        public Event Event { get; set; }  // Hangi Etkinlik?
        public int Quantity { get; set; } // Kaç Adet?

        // Toplam Fiyat (Birim Fiyat * Adet)
        // İndirim varsa indirimli fiyatı, yoksa normal fiyatı alır.
        public string SeatNumber { get; set; }
        public decimal TotalPrice
        {
            get
            {
                decimal finalPrice = Event.DiscountRate > 0 ? Event.DiscountedPrice : Event.Price;
                return finalPrice * Quantity;
            }
        }
    }
}