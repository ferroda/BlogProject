using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Models;

namespace Blog.Controllers
{

    /// <summary>
    /// Unused class. Without individual mode I must use this class.
    /// This class is controlling the entities data, listing us the datas from the BlogDatabase
    /// it will be visible in the View section. 
    /// </summary>
    public class BlogListController : Controller
    {
        BlogDatabaseEntities db = new BlogDatabaseEntities();

        public ActionResult BlogListView()
        {
            List<BlogPostModel> blogPostModelList = new List<BlogPostModel>();

            blogPostModelList = db.BlogPost.Select(bp => new BlogPostModel()
            {
                Title = bp.Title,
                Text = bp.Text
            }).ToList();


            return View(blogPostModelList);
        }
    }
}