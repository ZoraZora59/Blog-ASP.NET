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
            var templist = new List<string>();
            foreach(var item in time_list)
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
                time_lists.Add(temp);
            }
			
            ViewBag.timesort = time_lists;
            ViewBag.categroyList = templist;
			//评论排序
			ViewBag.TopComList = GetTopCmtLst(5);//设定评论排行榜的文章数量

			return View("~/Views/Shared/_Sidebar.cshtml", hots);
            
        }
		
		public List<TopCmtLst> GetTopCmtLst(int n)
		{
			//var TCL = new List<TopCmtLst>();
			//var tIDlst = db.TextLists.Select(c => c.TextID).ToList();
			//var txtCommitCount = new Dictionary<int, int>();
			//foreach (var item in tIDlst)
			//{
			//	txtCommitCount.Add(item, db.CommitLists.Count(c => c.TextID == item));
			//}
			//txtCommitCount = txtCommitCount.OrderByDescending(p => p.Value).ToDictionary(p => p.Key, o => o.Value);
			for (int i=0;i<n;i++)
			{
				
				TCL.Add(txtCommitCount.);
			}
			return TCL;
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