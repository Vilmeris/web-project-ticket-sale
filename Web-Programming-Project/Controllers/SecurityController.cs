using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Web_Programming_Project.Models;

namespace Web_Programming_Project.Controllers
{
    public class SecurityController : Controller
    {
        DbPersonal db = new DbPersonal();  

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            var userInDb = db.Users.FirstOrDefault(x => x.Email == user.Email && x.Password == user.Password);

            if (userInDb != null)
            {
                FormsAuthentication.SetAuthCookie(userInDb.Email, false);

                
                Session["NameSurname"] = userInDb.Name + " " + userInDb.Surname;

                
                Session["User"] = userInDb;

                Session["UserEmail"] = userInDb.Email;

                if (userInDb.Role == "Admin")
                {
                    return RedirectToAction("Index", "PersonelManage");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.Message = "Geçersiz E-posta veya Şifre";
                return View();
            }
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var checkUser = db.Users.FirstOrDefault(x => x.Email == model.Email);
                if (checkUser != null)
                {
                    ViewBag.Message = "Bu e-posta adresi zaten kullanılıyor.";
                    return View(model);
                }

                User newUser = new User();
                newUser.Name = model.Name;
                newUser.Surname = model.Surname;
                newUser.Phone = model.Phone;
                newUser.Email = model.Email;
                newUser.Password = model.Password;
                newUser.Role = "User";

                db.Users.Add(newUser);
                db.SaveChanges();

                
                FormsAuthentication.SetAuthCookie(newUser.Email, false);

                
                Session["User"] = newUser;
                Session["NameSurname"] = newUser.Name + " " + newUser.Surname;
                Session["UserEmail"] = newUser.Email;

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); 
            return RedirectToAction("Login");
        }

        [HttpGet]
        [Authorize]
        public ActionResult EditProfile()
        {
            
            var sessionUser = Session["User"] as User;
            string userEmail = "";

            if (sessionUser != null)
            {
                userEmail = sessionUser.Email;
            }
            else
            {
                userEmail = User.Identity.Name; 
            }

            var kullanici = db.Users.FirstOrDefault(x => x.Email == userEmail);

            if (kullanici == null)
            {
                return RedirectToAction("Login");
            }

            return View(kullanici);
        }

        [HttpPost]
        public ActionResult EditProfile(Web_Programming_Project.Models.User gelenKullanici)
        {
            
            using (var context = new Web_Programming_Project.Models.DbPersonal())
            {
                var mevcutKullanici = context.Users.Find(gelenKullanici.UserId);

                if (mevcutKullanici != null)
                {
                    
                    mevcutKullanici.Name = gelenKullanici.Name;
                    mevcutKullanici.Surname = gelenKullanici.Surname;
                    mevcutKullanici.Phone = gelenKullanici.Phone;

                    if (!string.IsNullOrEmpty(gelenKullanici.Password))
                    {
                        mevcutKullanici.Password = gelenKullanici.Password;
                    }

                    context.SaveChanges();

                    
                    Session["NameSurname"] = mevcutKullanici.Name + " " + mevcutKullanici.Surname;

                    
                    Session["User"] = mevcutKullanici;

                    return RedirectToAction("Index", "Home");
                }
            }

            return View(gelenKullanici);
        }
    }
}