using DemoBlog.App_Code;
using NewBeeBlog.App_Code;
using NewBeeBlog.Models;
using NewBeeBlog.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewBeeBlog.Controllers
{
    public class HomeController : Controller
    {

        private NewBeeBlogContext db = new NewBeeBlogContext();
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            var model = new SerializeTool().DeSerialize<BlogConfig>();
			ViewBag.Config = model;
        }

        public ActionResult Index()
        {
            var currentLoginUser = Session["loginuser"] == null ? null : (User)Session["loginuser"];
            ViewBag.currentLoginInfo = currentLoginUser;
			var models = new List<TextIndex>();
			var TextList = new List<TextList>();
			TextList = db.TextLists.ToList();
			foreach(var item in TextList)
			{
				var temp = new TextIndex();
				temp.TextID = item.TextID;
				temp.CommitCount=db.CommitLists.Count(c => c.TextID == item.TextID);
				temp.Text = item.Text;
				if (item.CategoryName == null)
					item.CategoryName = "未分类";
				temp.CategoryName = item.CategoryName;
				temp.TextTitle = item.TextTitle;
				temp.TextChangeDate = item.TextChangeDate;
				temp.Hot = item.Hot;
				models.Add(temp);
			}
            return View(models);
        }

        
        public ActionResult SearchResult(string searchthing)
        {
            var currentLoginUser = Session["loginuser"] == null ? null : (User)Session["loginuser"];
            ViewBag.currentLoginInfo = currentLoginUser;
            var blog = from m in db.TextLists
                       select m;
            //搜索
            var search_list = new List<TextIndex>();
            if (!string.IsNullOrEmpty(searchthing))
            {
                var s_blogs = blog.Where(m => m.TextTitle.Contains(searchthing) || m.Text.Contains(searchthing)).ToList();
                foreach (var item in s_blogs)
                {
                    var temp = new TextIndex();
                    temp.TextID = item.TextID;
                    temp.CommitCount = db.CommitLists.Count(c => c.TextID == item.TextID);
                    temp.Text = item.Text;
                    if (item.CategoryName == null)
                        item.CategoryName = "未分类";
                    temp.CategoryName = item.CategoryName;
                    temp.TextTitle = item.TextTitle;
                    temp.TextChangeDate = item.TextChangeDate;
                    temp.Hot = item.Hot;
                    search_list.Add(temp);
                }
                ViewBag.searchRes = search_list;
            }
            return View(search_list);
        }


        [HttpGet]
        public ActionResult ChangeInfo()
        {
			if(Session["loginuser"] == null)
				return Redirect("/home");
			try
			{
				var currentLoginUser = (User)Session["loginuser"];
				ViewBag.currentLoginInfo = currentLoginUser;
				return View();
			}
			catch (Exception)
			{
				return Redirect("/home");
			}
            
        }


        [HttpPost]
        public ActionResult ChangeInfo(ChangeUserInfo model)
        {
            if (ModelState.IsValid)
            //判断是否验证通过
            {
                string sessionValidCode = Session["validatecode"] == null ? string.Empty : Session["validatecode"].ToString();
                var currentLoginUser = Session["loginuser"] == null ? null : (User)Session["loginuser"];
                if (!model.Code.Equals(sessionValidCode))
                {
                    return Content("验证码输入错误");
                }
                try
                {
                    var odlModel = db.Users.FirstOrDefault(m => m.Account == currentLoginUser.Account);
                    odlModel.Name = model.Name;
                    odlModel.Account = currentLoginUser.Account;
                    odlModel.Password = md5tool.GetMD5(model.Password);
                    DbEntityEntry entry = db.Entry(odlModel);
                    entry.State = EntityState.Modified;
                    int res = db.SaveChanges();
                    //保存数据库 
                }
                catch (DbUpdateException)
                {
                    return Content("数据库更新出错");
                }
                catch (ObjectDisposedException)
                {
                    return Content("数据上下文连接已过期");
                }
                catch (InvalidOperationException)
                {
                    return Content("数据实体处理异常");
                }
                catch (Exception)
                {
                    //TODO:异常报告
                    return Content("数据库异常");
                    throw;
                }
            }
            return Redirect("/");
        }
		[HttpGet]
        public ActionResult Blog(int id)
        {
            var currentLoginUser = Session["loginuser"] == null ? null : (User)Session["loginuser"];
            ViewBag.currentLoginInfo = currentLoginUser;
            var model = db.TextLists.FirstOrDefault(m => m.TextID == id);
            model.Hot += 1;
            DbEntityEntry entry = db.Entry(model);
            entry.State = EntityState.Modified;
            int res = db.SaveChanges();

			//评论模块
			var temp = db.CommitLists.Where(c => c.TextID == id).ToList();
			var cmt = new List<ShowCommit>();
			int i = 1;
			foreach(var item in temp)
			{
				var tmp = new ShowCommit();
				tmp.Name = db.Users.Where(c => c.Account == item.Account).FirstOrDefault().Name;
				tmp.Date = item.CommitChangeDate.ToString("yyyy-MM-dd")+"  "+ item.CommitChangeDate.ToShortTimeString();
				tmp.Content = item.CommitText;
				tmp.Id = item.CommitID;
				tmp.Num = i;
				cmt.Add(tmp);
				i++;
			}
			ViewBag.CmtList = cmt;
            return View(model);
        }

		[HttpPost]
		public JsonResult AddCommit()
		{
			try
			{
				int sTextID = int.Parse(Request["TextID"]);
				string sAccount = Request["Account"].ToString();
				string sContent = Request["Content"].ToString();
				db.CommitLists.Add(new CommitList { TextID=sTextID ,Account=sAccount,CommitText=sContent});
				db.SaveChanges();
				return Json(null);
			}
			catch (Exception)
			{
				return Json(0);
			}
		}

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public FileResult ValidateCode()
        {
            ValidateCode vc = new ValidateCode();
            string code = vc.CreateValidateCode(4);
            Session["validatecode"] = code;//把数字保存在session中
            byte[] bytes = vc.CreateValidateGraphic(code);//根据数字转成二进制图片
            return File(bytes, @"image/jpeg");//返回一个图片jpg
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