using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity; 
using Web_Programming_Project.Models;

namespace Web_Programming_Project.Controllers
{
    public class AccountController : Controller
    {
        private DbPersonal db = new DbPersonal();


        [Authorize]
        public ActionResult Profile()
        {
            string currentIdentity = User.Identity.Name;


            var currentUser = db.Users.FirstOrDefault(x => x.Email == currentIdentity)
                           ?? db.Users.FirstOrDefault(x => x.Name == currentIdentity);

            if (currentUser == null) return RedirectToAction("Login");

            
            var myTickets = db.Tickets
                              .Include("Event")
                              .Where(t => t.UserId == currentUser.UserId)
                              .OrderByDescending(t => t.PurchaseDate)
                              .ToList();

            
            ViewBag.UserName = currentUser.Name + " " + currentUser.Surname;

            return View(myTickets);
        }
    }
}