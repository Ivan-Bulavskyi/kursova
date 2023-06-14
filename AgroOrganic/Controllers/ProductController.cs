using AgroOrganic.Models;
using System.Linq;
using System.Web.Mvc;

namespace AgroOrganic.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Post(int? id)
        {
            if (id == null )
            {
                return RedirectToAction("Index");
            }

            ViewBag.id = id;
            return View();
        }


        public JsonResult GetPost(int id)
        {
            PostContext context = new PostContext();
            var posts = context.Post.ToList().Where(postId => postId.Id == id)
                .Select(post => new
                {
                    Id = post.Id,
                    Title = post.Title,
                    Text = post.Text
                });
            return Json(posts, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetPosts()
        {
            PostContext context = new PostContext();
            var posts = context.Post.ToList().Where(post => post.CategoryId == 3)
                .Select(post => new
                {
                    Id = post.Id,
                    Title = post.Title,
                    Text = post.Text,
                    RoleId = post.RoleId
                }).OrderByDescending(r => r.RoleId);
            return Json(posts, JsonRequestBehavior.AllowGet);
        }
    }
}