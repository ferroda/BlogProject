using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Blog.Models;
using Blog;
using System.IO;

namespace Blog.Controllers
{
    // Generated class with custom methods.

    /// <summary>
    /// This class stand for maneging created blog posts (like create, delete or edit)
    /// </summary>
    /// 
    public class BlogManageController : Controller
    {
        private BlogDatabaseEntities db = new BlogDatabaseEntities();

        /// <summary>
        /// Clearing the session, if the user logging out.
        /// </summary>
        /// <returns></returns>
        public ActionResult ExitSession()
        {
            Session.Clear();

            return View();
        }

        /// <summary>
        /// Converting filebase to byte[].
        /// </summary>
        /// <param name="fileBase"></param>
        /// <returns></returns>
        private byte[] ConvertHttpPostedFileBaseToByteArray(HttpPostedFileBase fileBase)
        {
            byte[] returnData;

            using (MemoryStream imageMemoryStream = new MemoryStream())
            {
                fileBase.InputStream.CopyTo(imageMemoryStream);
                returnData = imageMemoryStream.GetBuffer();
            }

            return returnData;
        }

        /// <summary>
        /// Converting byte[] data to string.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string ConvertImageDataToSourceString(byte[] data)
        {
            return String.Format($"data:image/jpg;base64,{Convert.ToBase64String(data)}");
        }

        /// <summary>
        /// Controller for ReadPost view. 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ReadPost(int Id)
        {
            BlogPost blogPost = db.BlogPost.Find(Id);

            if (blogPost.Image != null)
            {
                ViewBag.SourceString = ConvertImageDataToSourceString(blogPost.Image);
            }

            return View(blogPost);
        }

        // GET: BlogPosts
        /// <summary>
        /// Listing the visible blogs for everyone.
        /// </summary>
        /// <returns></returns>
        public ActionResult BlogList()
        {
            int.TryParse(Session["Id"]?.ToString(), out int number);
            int? id = number;

            var blogPost = db.BlogPost.Include(b => b.Color).Include(b => b.Color1).Include(b => b.User).
                Where(bp => bp.IsVisible == true || id == bp.UserId);

            return View(blogPost.ToList());
        }

        /// <summary>
        /// Create-GET. 
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            if (Session["Id"] != null)  // Using if-else block, for find out is the user logged in or not.
            {
                ViewBag.FontColorId = new SelectList(db.Color, "Id", "Name");
                ViewBag.BackgroundColorId = new SelectList(db.Color, "Id", "Name");
                ViewBag.UserId = new SelectList(db.User, "Id", "Name");
                return View();
            }
            else
            {
                // The user don't log in yet, but want to write a blog, redirect him/her to the login section.
                TempData["postLogIn"] = "showMessage";
                return RedirectToAction("Login", "Account");
            }
        }

        /// <summary>
        ///  Create-POST. BlogPost--> Generated model, BlogPostModel-->Own model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        [HttpPost, ActionName("Create")]  //Using create get branch
        public ActionResult BlogText(BlogPostModel model)
        {
            //DB change--> Allow nulls --> UserId, IsVisible
            var blogPost = new BlogPost();

            // Using viewbag for get the colors for the view.
            ViewBag.FontColorId = new SelectList(db.Color, "Id", "Name");
            ViewBag.BackgroundColorId = new SelectList(db.Color, "Id", "Name");
            // Validation if the two color is the same.
            if (model.FontColorId == model.BackgroundColorId)
            {
                ModelState.AddModelError("FontColorId", "A háttér és a betű színe azonos!");

                return View(model);
            }

            if (ModelState.IsValid)
            {
                // Mapping here -->
                blogPost.UserId = int.Parse(Session["Id"].ToString()); // Write posts only logged in state.
                blogPost.Title = model.Title;
                blogPost.Text = model.Text;
                blogPost.IsVisible = model.IsVisible;
                blogPost.FontSize = model.FontSize;
                blogPost.FontColorId = model.FontColorId;
                blogPost.BackgroundColorId = model.BackgroundColorId;
                // Adding the image here.
                if (model.DataInHttpPostedFileBase != null && model.Image != null)
                {
                    blogPost.Image = ConvertHttpPostedFileBaseToByteArray(model.DataInHttpPostedFileBase);
                }

                db.BlogPost.Add(blogPost);
                db.SaveChanges();

                // Using TempData for JavaScript (show alert message)
                //..Scripts/MessageForUser
                TempData["postSaved"] = "showMessage";


                return RedirectToAction("BlogList");
            }

            return View("Create"); // Else--> The validation failed.
        }

        
        // GET: BlogPosts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BlogPost blogPost = db.BlogPost.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }

            ViewBag.FontColorId = new SelectList(db.Color, "Id", "Name", blogPost.FontColorId);
            ViewBag.BackgroundColorId = new SelectList(db.Color, "Id", "Name", blogPost.BackgroundColorId);
            ViewBag.UserId = new SelectList(db.User, "Id", "Name", blogPost.UserId);


            return View(blogPost);
        }

        // POST: BlogPosts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Text,FontSize,FontColorId,BackgroundColorId,IsVisible")] BlogPost blogPost)
        {
            if (ModelState.IsValid)
            {
                BlogPost dbBlogPost = db.BlogPost.Find(blogPost.Id);
                dbBlogPost.Text = blogPost.Text;
                dbBlogPost.Title = blogPost.Title;
                dbBlogPost.IsVisible = blogPost.IsVisible;
                dbBlogPost.BackgroundColorId = blogPost.BackgroundColorId;
                dbBlogPost.FontColorId = blogPost.FontColorId;

                db.Entry(dbBlogPost).State = EntityState.Modified;
                db.SaveChanges();


                //If the post is edited using a model(..Scripts/MessageForUser)
                TempData["postEdited"] = "showMessage";
                return RedirectToAction("../Home/Index");
            }

            ViewBag.FontColorId = new SelectList(db.Color, "Id", "Name", blogPost.FontColorId);
            ViewBag.BackgroundColorId = new SelectList(db.Color, "Id", "Name", blogPost.BackgroundColorId);
            ViewBag.UserId = new SelectList(db.User, "Id", "Name", blogPost.UserId);


            return View(blogPost);
        }

        // GET: BlogPosts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.BlogPost.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BlogPost blogPost = db.BlogPost.Find(id);
            db.BlogPost.Remove(blogPost);
            db.SaveChanges();


            // Show the message, if the user deleting a post.
            TempData["postDeleted"] = "showMessage";

            return RedirectToAction("../Home/Index");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
