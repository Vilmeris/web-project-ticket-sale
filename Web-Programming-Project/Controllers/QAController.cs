using System;
using System.Linq;
using System.Web.Mvc;
using Web_Programming_Project.Models;

namespace Web_Programming_Project.Controllers
{
    public class QAController : Controller
    {
        DbPersonal db = new DbPersonal();

        
        public ActionResult Index()
        {
            var sorular = db.Questions.OrderByDescending(x => x.Date).ToList();
            return View(sorular);
        }

       
        [HttpPost]
        public ActionResult Ask(string questionText)
        {
            if (!string.IsNullOrEmpty(questionText))
            {
                Question q = new Question();
                q.Text = questionText;
                q.Date = DateTime.Now;

                
                if (Session["NameSurname"] != null)
                    q.AskerName = Session["NameSurname"].ToString();
                else
                    q.AskerName = "Misafir İzleyici";

                db.Questions.Add(q);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        
        [HttpPost]
        public ActionResult Answer(int id, string answerText)
        {
        

            var soru = db.Questions.Find(id);
            if (soru != null)
            {
                soru.Answer = answerText;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}