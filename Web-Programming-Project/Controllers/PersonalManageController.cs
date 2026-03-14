using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Web_Programming_Project.Models;

namespace Web_Programming_Project.Controllers
{
    [Authorize]  
    public class PersonelManageController : Controller
    {
        private DbPersonal db = new DbPersonal(); 

       
        public ActionResult Index()
        {
            
            string currentEmail = User.Identity.Name;

            
            var currentUser = db.Users.FirstOrDefault(x => x.Email == currentEmail);

           
            if (currentUser != null && currentUser.Role == "Admin")
            {
                
                return View(db.Users.ToList());
            }
            else
            {
               
                return RedirectToAction("Index", "Home");
            }
        }

    
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

 
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Create([Bind(Include = "UserId,Name,Surname,Phone,Email,Password,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                // E-posta kontrolü
                var checkUser = db.Users.FirstOrDefault(x => x.Email == user.Email);
                if (checkUser != null)
                {
                    ViewBag.Message = "Bu e-posta zaten kayıtlı!";
                    return View(user);
                }

                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

      
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,Name,Surname,Phone,Email,Password,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

      
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult MyProfile()
        {
            var userEmail = User.Identity.Name;
            var user = db.Users.FirstOrDefault(x => x.Email == userEmail);

            if (user == null) return RedirectToAction("Login", "Security");

            
            ViewBag.CreditCards = db.CreditCards.Where(x => x.UserId == user.UserId).ToList();

            
            ViewBag.MyTickets = db.Tickets.Include(t => t.Event).Where(t => t.UserId == user.UserId).ToList();

            return View(user);
        }

        public ActionResult AddCreditCard()
        {
            return View();
        }

        // 2. Kredi Kartı Ekleme (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCreditCard(CreditCard card)
        {
            string email = User.Identity.Name;
            var user = db.Users.FirstOrDefault(x => x.Email == email);

            if (user != null && ModelState.IsValid)
            {
                card.UserId = user.UserId;
                card.CardLimit = 1000000; 
                db.CreditCards.Add(card);
                db.SaveChanges();
                TempData["Success"] = "Kart başarıyla eklendi.";
                return RedirectToAction("MyProfile");
            }
            return View(card);
        }

        // 3. Cüzdana Para Yükleme
        [HttpPost]
        public ActionResult LoadMoney(decimal amount, int cardId)
        {
            string email = User.Identity.Name;
            var user = db.Users.FirstOrDefault(x => x.Email == email);
            var card = db.CreditCards.FirstOrDefault(c => c.CardId == cardId && c.UserId == user.UserId);

            if (user != null && card != null)
            {
                if (card.CardLimit >= amount)
                {
                    card.CardLimit -= amount;
                    user.WalletBalance += amount; 
                    db.SaveChanges();
                    TempData["Success"] = amount + " TL cüzdanınıza yüklendi.";
                }
                else
                {
                    TempData["Error"] = "Kart limiti yetersiz.";
                }
            }
            return RedirectToAction("MyProfile");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}