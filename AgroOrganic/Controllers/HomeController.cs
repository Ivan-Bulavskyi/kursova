using AgroOrganic.Models;
using System.Web.Mvc;
using System.Collections.Generic;
using AgroOrganic.Models.MapModels;
using System.Linq;
using System.Data.Entity;

namespace AgroOrganic.Controllers
{
    public class HomeController : Controller
    {
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
        SubscribeContext subDB = new SubscribeContext();
        FeedbackContext feedDB = new FeedbackContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult Team()
        {
            return View();
        }
        public ActionResult Technology()
        {
            return View();
        }
        public ActionResult Certification()
        {
            return View();
        }
        public ActionResult Processing()
        {
            return View("~/Views/Home/Processing.cshtml");
        }
        public ActionResult Map()
        {
            return View();
        }

        public ActionResult map_int()
        {
            return View();
        }

        public ActionResult maps()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Subscribe(Subscribe subscribe)
        {
            if (ModelState.IsValid)
            {
                subDB.Subscribe.Add(subscribe);
                subDB.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                feedDB.Feedback.Add(feedback);
                feedDB.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}