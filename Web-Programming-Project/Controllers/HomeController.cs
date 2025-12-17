using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Programming_Project.Models;

namespace Web_Programming_Project.Controllers
{
    public class HomeController : Controller
    {
        private DbPersonal db = new DbPersonal();
        public ActionResult Index()
        {
            var events = db.Events.Where(e => e.Date > DateTime.Now).OrderBy(e => e.Date).ToList();
            return View(events);

        }

        public ActionResult About()
        {
            ViewBag.Message = "About Page";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact Page.";

            return View();
        }

        public ActionResult Category(string id)
        {
            DbPersonal db = new DbPersonal();

            var events = db.Events.Where(e => e.Category.Contains(id)).ToList();

            ViewBag.CategoryName = id.ToUpper();

            return View(events);
        }


        public ActionResult Details(int id)
        {
            // 1. Etkinliği bul
            var eventItem = db.Events.Find(id);

            if (eventItem == null)
            {
                return RedirectToAction("Index");
            }

            // --- EKSİK OLAN KISIM BURASIYDI ---
            // Etkinliğin fiyat seçeneklerini veritabanından çekip ViewBag'e atıyoruz.
            // Eğer bunu yapmazsak View tarafında liste boş kalır.
            var priceOptions = db.EventPrices.Where(x => x.EventId == id).ToList();
            ViewBag.PriceOptions = priceOptions;
            // ----------------------------------

            // Özel tasarımlı View'i çağır
            return View("~/Views/Events/Details_User.cshtml", eventItem);
        }

       
    }
}