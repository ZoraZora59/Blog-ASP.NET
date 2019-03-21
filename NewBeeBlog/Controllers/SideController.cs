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

            //最新博文，时间排序
            var time_lists = new List<TextListsHot>();
            var time_list = blog.OrderByDescending(m => m.TextChangeDate).Take(2).ToList();
            foreach(var item in time_list)
            {
                var temp = new TextListsHot();
                temp.TextID = item.TextID;
                temp.TextTitle = item.TextTitle;
                temp.Hot = item.Hot;
                temp.CategoryName = item.CategoryName;
                temp.Datemouth = item.TextChangeDate.ToString().Substring(0, 6);
                time_lists.Add(temp);
            }
            ViewBag.timesort = time_lists;
            
            //分类查找
            
            var cate_list = blog.ToList();
            var templist = new List<string>();
            foreach (var item in cate_list)
            {
                var temp = new TextListsHot();
                temp.TextID = item.TextID;
                temp.TextTitle = item.TextTitle;
                temp.Hot = item.Hot;
                if (!templist.Contains(item.CategoryName))
                {
                    templist.Add(item.CategoryName);
                }
                temp.Datemouth = item.TextChangeDate.ToString().Substring(0, 6);
            }
            ViewBag.categroyList = templist;
            
            //最新评论
            //最热评论
            
            

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