using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Programming_Project.Models
{
    public class CartItem
    {
        public Event Event { get; set; }  
        public int Quantity { get; set; } 

       
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