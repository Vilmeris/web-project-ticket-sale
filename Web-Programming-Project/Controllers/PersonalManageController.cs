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
    [Authorize]  // authorizing for only logged in users
    public class PersonelManageController : Controller
    {
        private DbPersonal db = new DbPersonal(); // databese for our users

        // ------------------- LİSTELEME (INDEX) -------------------
        // GET: PersonalManage
        public ActionResult Index()
        {
            // 1. Giriş yapan kişinin e-postasını al
            string currentEmail = User.Identity.Name;

            // 2. Bu kişiyi veritabanında bul
            var currentUser = db.Users.FirstOrDefault(x => x.Email == currentEmail);

            // 3. GÜVENLİK KONTROLÜ: Kişi "Admin" mi?
            if (currentUser != null && currentUser.Role == "Admin")
            {
                // Admin ise listeyi göster
                return View(db.Users.ToList());
            }
            else
            {
                // Admin değilse Ana Sayfaya gönder
                return RedirectToAction("Index", "Home");
            }
        }

        // ------------------- DETAYLAR (DETAILS) -------------------
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

        // ------------------- YENİ KULLANICI EKLEME (CREATE) -------------------
        // Admin panelinden manuel kullanıcı eklemek için
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // Bind içindeki alanları senin User modeline göre güncelledim
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

        // ------------------- DÜZENLEME (EDIT) -------------------
        // Admin'in kullanıcı rollerini veya bilgilerini değiştirmesi için
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

        // ------------------- SİLME (DELETE) -------------------
        // GET: Silme onay sayfası
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

        // POST: Silme işlemini onayla ve bitir
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
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