using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewBeeBlog.Models;

namespace NewBeeBlog.Controllers
{
    public class SideController : Controller
    {
        [ChildActionOnly]
        public ActionResult Sidebar()
        {
            using (NewBeeBlogContext db = new NewBeeBlogContext())
            {

                var model = db.TextLists.ToList().OrderByDescending(m=>m.Hot).ToList();
               
                return View("~/Views/Shared/_Sidebar.cshtml", model);
            }
            
        }
    }
}