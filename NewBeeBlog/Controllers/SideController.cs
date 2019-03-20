using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewBeeBlog.App_Code;
using NewBeeBlog.Models;
using NewBeeBlog.ViewModels;

namespace NewBeeBlog.Controllers
{
    
    public class SideController : Controller
    {
        private NewBeeBlogContext db = new NewBeeBlogContext();

        [ChildActionOnly]
        public ActionResult Sidebar(string searchSthing)
        {
            var model = new SerializeTool().DeSerialize<BlogConfig>();
            ViewBag.Config = model;

            var hots = new List<TextListsHot>();
            var blog = from m in db.TextLists
                       select m;
            //热度排序
            var list = blog.OrderByDescending(m => m.Hot).Take(5).ToList();
            foreach(var item in list)
            {
                var temp = new TextListsHot();
                temp.TextID = item.TextID;
                temp.TextTitle = item.TextTitle;
                temp.Hot = item.Hot;
                temp.Datemouth = item.TextChangeDate.ToString().Substring(0,6);
                hots.Add(temp);
            }

            //时间排序
            var time_lists = new List<TextListsHot>();
            var time_list = blog.OrderByDescending(m => m.TextChangeDate).ToList();
            foreach(var item in time_list)
            {
                var temp = new TextListsHot();
                temp.TextID = item.TextID;
                temp.TextTitle = item.TextTitle;
                temp.Hot = item.Hot;
                temp.Datemouth = item.TextChangeDate.ToString().Substring(0, 6);
                time_lists.Add(temp);
            }
            ViewBag.timesort = time_lists;

            
            

            return View("~/Views/Shared/_Sidebar.cshtml", hots);
            
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