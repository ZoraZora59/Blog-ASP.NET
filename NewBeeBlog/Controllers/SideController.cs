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
        private NewBeeBlogContext db = new NewBeeBlogContext();
        [ChildActionOnly]
        public ActionResult Sidebar()
        {

            var blog = from m in db.TextLists
                       select m;

            var model = blog.OrderByDescending(m => m.Hot).ToList();

            return View("~/Views/Shared/_Sidebar.cshtml", model);
            


        }
        protected override void Dispose(bool disposing)//数据连接释放
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
    
}