using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Programming_Project.Models;

namespace Web_Programming_Project.Controllers
{
    public class CartController : Controller
    {
        private DbPersonal db = new DbPersonal();

        // -----------------------------------------------------------
        // 1. SEPETİ GÖRÜNTÜLE (INDEX)
        // -----------------------------------------------------------
        public ActionResult Index()
        {
            var cart = GetCart();

            // Toplam Tutarı Hesapla (ViewBag ile sayfaya taşıyoruz)
            // CartItem modelindeki "TotalPrice" özelliğini kullanıyoruz
            decimal grandTotal = 0;
            if (cart.Any())
            {
                grandTotal = cart.Sum(x => x.TotalPrice);
            }

            ViewBag.GrandTotal = grandTotal;

            return View(cart);
        }

        // -----------------------------------------------------------
        // 2. SEPETTEN SİL (REMOVE)
        // -----------------------------------------------------------
        // Not: Sadece ID yetmez, Koltuk Numarası da lazım! 
        // Yoksa aynı etkinlikten 2 bilet varsa hangisini sileceğini bilemez.
        public ActionResult Remove(int eventId, string seatNumber)
        {
            var cart = GetCart();

            // Hem EventId hem de Koltuk Numarası eşleşen kaydı bul
            var itemToRemove = cart.FirstOrDefault(x => x.Event.EventId == eventId && x.SeatNumber == seatNumber);

            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);
                Session["Cart"] = cart; // Güncel listeyi kaydet
            }

            return RedirectToAction("Index");
        }

        // -----------------------------------------------------------
        // 3. ÖDEME YAP / SATIN AL (CHECKOUT)
        // -----------------------------------------------------------
        [HttpPost]
        public ActionResult Checkout()
        {
            var cart = GetCart();

            if (cart.Count == 0)
            {
                TempData["Error"] = "Sepetiniz boş!";
                return RedirectToAction("Index");
            }

            // Sepetteki her bir ürün için veritabanına kayıt açıyoruz
            foreach (var item in cart)
            {
                // Veritabanındaki etkinliği bul (Fiyat/Stok kontrolü için güncel veri)
                var eventInDb = db.Events.Find(item.Event.EventId);

                if (eventInDb != null)
                {
                    // Ticket Modeline kayıt ekle
                    var ticket = new Ticket
                    {
                        EventId = item.Event.EventId,
                        SeatNumber = item.SeatNumber,
                        PurchaseDate = DateTime.Now,
                        PricePaid = item.TotalPrice // O anki fiyatı kaydet
                        // UserId = User.Identity.GetUserId() // Eğer login varsa burayı açabilirsin
                    };

                    db.Tickets.Add(ticket);

                    // Satılan bilet sayısını artır
                    eventInDb.SoldTicketCount++;
                }
            }

            // Tüm değişiklikleri veritabanına işle
            db.SaveChanges();

            // Sepeti Boşalt
            Session["Cart"] = null;

            TempData["Success"] = "Ödeme başarıyla alındı! İyi eğlenceler.";

            // Başarı sayfasına veya Ana sayfaya yönlendir
            return RedirectToAction("Index", "Home");
        }

        // -----------------------------------------------------------
        // 4. SEPETİ TEMİZLE (HEPSİNİ SİL)
        // -----------------------------------------------------------
        public ActionResult ClearCart()
        {
            Session["Cart"] = null;
            return RedirectToAction("Index");
        }

        // -----------------------------------------------------------
        // YARDIMCI METOT (PRIVATE)
        // -----------------------------------------------------------
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