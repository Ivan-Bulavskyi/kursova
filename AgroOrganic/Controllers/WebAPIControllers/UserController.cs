using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AgroOrganic.Models;

namespace AgroOrganic.Controllers.WebAPIControllers
{
    public class UserController : ApiController
    {
        [HttpPost]
        [Route("api/user/subscribe")]
        public string Subscribe(Subscribe email)
        {
            if (ModelState.IsValid)
            {
                var context = new SubscribeContext();
                context.Subscribe.Add(email);
                context.SaveChanges();
                return "success";
            }
            return "fail";
        }

        [HttpPost]
        [Route("api/user/feedback")]
        public string Feedback(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                var context = new FeedbackContext();
                context.Feedback.Add(feedback);
                context.SaveChanges();
                return "success";
            }
            return "fail";
        }
    }
}
