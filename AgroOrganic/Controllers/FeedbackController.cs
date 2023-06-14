using AgroOrganic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AgroOrganic.Controllers
{
    public class FeedbackController : Controller
    {
        FeedbackContext db = new FeedbackContext();

        // GET: Feedback
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Create(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                db.Feedback.Add(feedback);
                db.SaveChanges();
                
            }
            return RedirectToAction("Index");
        }
    }
}