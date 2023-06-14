using AgroOrganic.Models;
using System.Web.Mvc;

namespace AgroOrganic.Controllers
{
    public class WelcomeController : Controller
    {
        SubscribeContext subDB = new SubscribeContext();
        FeedbackContext feedDB = new FeedbackContext();
        // GET: Welcome
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Subscribe(Subscribe subscribe)
        {
            if (ModelState.IsValid)
            {
                subDB.Subscribe.Add(subscribe);
                subDB.SaveChanges();
            }
            return Json(new { Email = subscribe.Email });
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Contact(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                feedDB.Feedback.Add(feedback);
                feedDB.SaveChanges();
            }
            return Json(new { Email = feedback.Email });
        }
    }
}