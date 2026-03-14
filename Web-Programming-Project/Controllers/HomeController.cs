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



            var priceOptions = db.EventPrices.Where(x => x.EventId == id).ToList();
            ViewBag.PriceOptions = priceOptions;

            return View("~/Views/Events/Details_User.cshtml", eventItem);
        }

        public JsonResult LiveSearch(string term)
        {

            if (string.IsNullOrEmpty(term)) return Json(null, JsonRequestBehavior.AllowGet);


            var results = db.Events
                .Where(x => x.Title.Contains(term) ||
                            x.Category.Contains(term) ||
                            x.SubCategory.Contains(term) ||
                            x.Artist.Contains(term))
                .Select(x => new {
                    id = x.EventId,
                    label = x.Title,
                    category = x.Category,
                    image = x.ImageUrl 
                })
                .Take(5) 
                .ToList();

            return Json(results, JsonRequestBehavior.AllowGet);
        }

     
        public ActionResult Search(string query)
        {

            if (string.IsNullOrWhiteSpace(query) || query.Trim().Length < 3)
            {
                ViewBag.ArananKelime = query;
                ViewBag.SonucSayisi = 0;
                return View(new List<Web_Programming_Project.Models.Event>());
            }

            var model = db.Events
                .Where(x => x.Title.Contains(query) ||
                            x.Category.Contains(query) ||
                            x.SubCategory.Contains(query) ||
                            x.Artist.Contains(query))
                .ToList();

            ViewBag.ArananKelime = query;
            ViewBag.SonucSayisi = model.Count;

            return View(model);
        }

        public ActionResult Privacy()
        {
            return View();
        }

        public ActionResult Help()
        {
            return View();
        }



    }
}