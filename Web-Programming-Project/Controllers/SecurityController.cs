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

        DbPersonal db = new DbPersonal();  // database for the project


        [HttpGet]
        public ActionResult Login()  // login action
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)  // Login actionresult for user object from User class
        {
            var userInDb = db.Users.FirstOrDefault(x => x.Email == user.Email && x.Password == user.Password);  // searching database for given email and password when logging


            if (userInDb != null)
            {
                FormsAuthentication.SetAuthCookie(userInDb.Email, false);  // create cookie when logged in ( kontrol et !! )

                Session["NameSurname"] = userInDb.Name + " " + userInDb.Surname;  // to displaying the name of user at main page

                if (userInDb.Role == "Admin")  // If the logged in user is admin, redirect to User List Page
                {
                    return RedirectToAction("Index", "PersonelManage");
                }
                else
                {

                    return RedirectToAction("Index", "Home");  // if it is not admin, redirect to main page
                }
            }
            else
            {
                ViewBag.Message = "Invalid E-mail or Password";  // else , warning the user
                return View();
            }
        }


        [HttpGet]
        public ActionResult Register()  // Register actionresult with httpget
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model) // Register actionresult for Httppost and uses object
        {
            if (ModelState.IsValid)  // Burası ne bilmiyorum
            {
                var checkUser = db.Users.FirstOrDefault(x => x.Email == model.Email);  // to check if there is registered user with given email
                if (checkUser != null)  // if there is email found
                {
                    ViewBag.Message = "This email address is already used.";  // 
                    return View(model); // return the object
                }

                User newUser = new User();
                newUser.Name = model.Name;  // get the information of users from Viewmodel model object
                newUser.Surname = model.Surname;
                newUser.Phone = model.Phone;
                newUser.Email = model.Email;
                newUser.Password = model.Password;

                newUser.Role = "User";  // every user is a normal user at first

                db.Users.Add(newUser);  // add new user to the Users database
                db.SaveChanges();  // save added user

                return RedirectToAction("Index","Home");  // after logging in, redirect user to main page 
            }

            return View(model);
        }

        public ActionResult Logout()  // Log out actionresult
        {
            FormsAuthentication.SignOut();  // bu ne bilmiyorum
            return RedirectToAction("Login"); // redirect to login page after logging out 
        }
    }
}