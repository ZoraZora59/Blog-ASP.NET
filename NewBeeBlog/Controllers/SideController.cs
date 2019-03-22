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
            var commit = from c in db.CommitLists
                         select c;
            var Ctime_list = commit.OrderByDescending(m => m.CommitChangeDate).Take(1).ToList();
            var tempC = new ShowCommit();
            var users = from u in db.Users
                        select u;
            foreach (var item in Ctime_list)
            {
                var textT = blog.Where(m => m.TextID == item.TextID).ToList();
                var NameC = users.Where(m => m.Account == item.Account).ToList();
                tempC.TextId = item.TextID;
                tempC.Content = item.CommitText;
                tempC.TextTitle = textT[0].TextTitle;
                tempC.Name = NameC[0].Name;
                tempC.Date = item.CommitChangeDate.ToString();
            }
           
            ViewBag.newestCom= tempC;

            ViewBag.TopComList = GetTopCmtLst(5);//设定评论排行榜的文章数量

            return View("~/Views/Shared/_Sidebar.cshtml", hots);
            
        }

		public List<TopCmtLst> GetTopCmtLst(int n)
		{
			var lTxt = db.TextLists.ToList();
			if (lTxt.Count == 0)
				return null;
			List<TopCmtLst> TCL = new List<TopCmtLst>();
			foreach (var item in lTxt)
			{
				TCL.Add(new TopCmtLst { TextID = item.TextID, TextTitle = item.TextTitle, CmtCount = db.CommitLists.Where(c => c.TextID == item.TextID).Count() });
			}
			if (TCL.Count == 0)
				return null;
			TCL=TCL.OrderByDescending(c => c.CmtCount).ToList();
			n = n > TCL.Count ? TCL.Count : n;
			for (int i = 0; i < n ; i++)
			{
				TCL[i].Num = i + 1;
			}
			
			return TCL.Take(n).ToList();
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