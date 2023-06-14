using AgroOrganic.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using Antlr.Runtime;
using System.IO;
using Microsoft.Owin;

namespace AgroOrganic.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        
        // GET: Admin
        public ActionResult Index()
        {
            
            var json = getUsers();

            return View();
        }

        public ActionResult Users()
        {
            var context = new ApplicationDbContext();
            var users = context.Users;
            var roles = context.Roles;
            ViewBag.Users = users.ToList();
            ViewBag.Roles = roles.ToList();
            return View();
        }

        public ActionResult Posts(string id, int? postId)
        {
            if (id?.ToLower() == "create")
                return View("~/Views/Admin/CreatePost.cshtml");
            if (id?.ToLower() == "edit")
            {
                ViewBag.postId = postId;
                return View("~/Views/Admin/EditPost.cshtml");
            }
            return View();
        }


        [HttpPost]
        public ActionResult Posts(Post post)
        {
            ModelState.Remove("id");
            if(ModelState.IsValid)
            {
                var context = new PostContext();
                var last_role_id = context.Post.ToList().Where(p => p.CategoryId == post.CategoryId)
                    .OrderByDescending(p => p.RoleId).FirstOrDefault();
                post.RoleId = last_role_id.RoleId + 1;
                
                context.Post.Add(post);
                context.SaveChanges();
            }
            return RedirectToAction("Posts");
        }

        [HttpPost]
        public string MoveUp(Post post)
        {
            var context = new PostContext();
            var currPost = context.Post.FirstOrDefault(pId => pId.Id == post.Id);
            var upperPost =
                context.Post.Where(
                        newpost => newpost.CategoryId == currPost.CategoryId && newpost.RoleId > currPost.RoleId)
                    .OrderBy(firstpost => firstpost.RoleId).FirstOrDefault();
            if (upperPost == null || currPost == null)
            {
                return "";
            }

            int tmp = currPost.RoleId;
            currPost.RoleId = upperPost.RoleId;
            upperPost.RoleId = tmp;
            context.SaveChanges();
            return "";
        }

        [HttpPost]
        public string MoveDown(Post post)
        {
            var context = new PostContext();
            var currPost = context.Post.FirstOrDefault(pId => pId.Id == post.Id);
            var upperPost =
                context.Post.Where(
                        newpost => newpost.CategoryId == currPost.CategoryId && newpost.RoleId < currPost.RoleId)
                    .OrderByDescending(firstpost => firstpost.RoleId).FirstOrDefault();
            if (upperPost == null || currPost == null)
            {
                return "";
            }

            int tmp = currPost.RoleId;
            currPost.RoleId = upperPost.RoleId;
            upperPost.RoleId = tmp;
            context.SaveChanges();
            return "";
        }

        [HttpPost]
        public string EditPost(Post post)
        {
            var context = new PostContext();
            var p = context.Post.FirstOrDefault(pId => pId.Id == post.Id);
            if (p == null) return "fail";
            p.CategoryId = post.CategoryId;
            p.Text = post.Text;
            p.Title = post.Title;
            context.SaveChanges();

            return "success";
        }

        [HttpPost]
        public string DeletePost(string postId)
        {
            var context = new PostContext();
            var del = context.Post.Find(Int32.Parse(postId));
            context.Post.Remove(del);
            context.SaveChanges();

            return "success";
        }

        [HttpPost]
        public JsonResult UploadFile()
        {
            bool isSavedSuccessfully = true;
            string pathString = "";
            string fName = "";
            HttpPostedFileBase file = null;
            try
            {
                foreach (string fileName in Request.Files)
                {
                    file = Request.Files[fileName];
                    //Save file content goes here
                    fName = file?.FileName;
                    if (file?.ContentLength > 0)
                    {
                    
                        var originalDirectory = new DirectoryInfo(string.Format("{0}Uploads", Server.MapPath(@"\")));
                    
                        pathString = Path.Combine(originalDirectory.ToString(), "posts");
                    
                        var fileName1 = Path.GetFileName(file.FileName);
                    
                        bool isExists = Directory.Exists(pathString);
                    
                        if (!isExists)
                            Directory.CreateDirectory(pathString);
                    
                        var path = string.Format("{0}\\{1}", pathString, file.FileName);
                        file.SaveAs(path);
                    
                    }
                    
                }

            }
            catch (Exception ex)
            {
                isSavedSuccessfully = false;
            }

            
            if (isSavedSuccessfully)
            {
                return Json(new { Result = "Success", Link = "http://" + Request?.Url?.Authority + "/Uploads/posts/" + file?.FileName});
            }
            else
            {
                return Json(new { Result = "Error" });
            }
        }


        [AllowAnonymous]
        public JsonResult getPost(int? id)
        {
            var context = new PostContext();
            var post = context.Post.ToList().Where(postId => postId.Id == id)
                .Select(p => new
                {
                    Id = p.Id,
                    Title = p.Title,
                    Text = p.Text,
                    CategoryId = p.CategoryId,
                    RoleId = p.RoleId
                });
            return Json(post, JsonRequestBehavior.AllowGet);
        }


        public JsonResult getPosts()
        {
            var context = new PostContext();
            var posts = context.Post.ToList().Select(p => new
            {
                Id       = p.Id,
                Category = p.CategoryId,
                Title    = p.Title,
                RoleId   = p.RoleId
            }).OrderByDescending(r => r.Id);
            return Json(posts, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getUsers()
        {
            var context = new ApplicationDbContext();
            var users = context.Users.ToList().Select(p => new {
                Email = p.Email,
                Name = p.UserName,
                Role = p.Roles.ToList().Select(r => new { r.RoleId })
            });


            return Json(users, JsonRequestBehavior.AllowGet);
        }

        

    }
}