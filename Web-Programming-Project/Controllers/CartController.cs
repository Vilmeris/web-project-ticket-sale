using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Web_Programming_Project.Models;


namespace Web_Programming_Project.Controllers
{
    public class CartController : Controller
    {
        private DbPersonal db = new DbPersonal();

        public ActionResult Index()
        {
            var cart = GetCart();

            
            foreach (var item in cart)
            {
                
                decimal basePrice = item.Event.Price;

              
                if (item.SeatNumber == "VIP")
                {
                    item.Event.Price = basePrice * 2;
                }
                else if (item.SeatNumber == "Ogrenci")
                {
                    
                    item.Event.Price = basePrice * 0.8m;
                }
                
            }

            
            decimal grandTotal = 0;
            if (cart.Any())
            {
                
                grandTotal = cart.Sum(x => x.Event.Price * x.Quantity);
            }

            ViewBag.GrandTotal = grandTotal;

            return View(cart);
        }

      
        public ActionResult Remove(int eventId, string seatNumber)
        {
            var cart = GetCart();

            var itemToRemove = cart.FirstOrDefault(x => x.Event.EventId == eventId && x.SeatNumber == seatNumber);

            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);
                Session["Cart"] = cart; 
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        [Authorize]
        public ActionResult Checkout(string cardHolder, string cardNumber, string expiryDate, string cvc)
        {
            
            if (string.IsNullOrEmpty(cardNumber) || string.IsNullOrEmpty(cvc) || string.IsNullOrEmpty(cardHolder))
            {
                TempData["Error"] = "Lütfen kart bilgilerini eksiksiz girin.";
                return RedirectToAction("Index");
            }

            var cart = GetCart();
            if (cart.Count == 0)
            {
                TempData["Error"] = "Sepetiniz boş!";
                return RedirectToAction("Index");
            }

          
            string currentIdentity = User.Identity.Name;
            var currentUser = db.Users.FirstOrDefault(x => x.Email == currentIdentity)
                           ?? db.Users.FirstOrDefault(x => x.Name == currentIdentity);

            if (currentUser == null)
            {
                TempData["Error"] = "Kullanıcı oturumu doğrulanamadı.";
                return RedirectToAction("Login", "Security");
            }

            
            try
            {
                foreach (var item in cart)
                {
                   
                    var eventInDb = db.Events.Find(item.Event.EventId);

                    if (eventInDb != null)
                    {
                        
                        if (eventInDb.SoldTicketCount + item.Quantity > eventInDb.Capacity)
                        {
                            TempData["Error"] = $"Üzgünüz, '{eventInDb.Title}' etkinliği için yeterli boş koltuk kalmadı.";
                            return RedirectToAction("Index");
                        }

                        
                        decimal finalPrice = eventInDb.Price; 

                        if (item.SeatNumber == "VIP")
                        {
                            finalPrice = eventInDb.Price * 2;
                        }
                        else if (item.SeatNumber == "Ogrenci")
                        {
                            finalPrice = eventInDb.Price * 0.8m;
                        }

                       
                        for (int i = 0; i < item.Quantity; i++)
                        {
                            var ticket = new Ticket
                            {
                                EventId = eventInDb.EventId,
                                UserId = currentUser.UserId,
                                SeatNumber = item.SeatNumber, 
                                PurchaseDate = DateTime.Now,
                                PricePaid = finalPrice 
                            };

                            db.Tickets.Add(ticket);
                        }

                        
                        eventInDb.SoldTicketCount += item.Quantity;
                    }
                }

                
                db.SaveChanges();

               
                Session["Cart"] = null;

                TempData["Success"] = "Ödeme başarıyla alındı! Biletleriniz profilinize eklendi.";
                return RedirectToAction("Profile", "Account");
            }
            catch (Exception ex)
            {
               
                TempData["Error"] = "Bir hata oluştu: " + ex.Message;
                return RedirectToAction("Index");
            }
        }




      
        public ActionResult ClearCart()
        {
            Session["Cart"] = null;
            return RedirectToAction("Index");
        }

        
        private List<CartItem> GetCart()
        {
            var cart = Session["Cart"] as List<CartItem>;
            if (cart == null)
            {
                cart = new List<CartItem>();
                Session["Cart"] = cart;
            }
            return cart;
        }
    }
}