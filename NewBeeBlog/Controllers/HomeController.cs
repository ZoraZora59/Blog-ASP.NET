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
            return View();
        }

        [HttpGet]
        public ActionResult ChangeInfo()
        {
            var currentLoginUser = Session["loginuser"] == null ? null : (User)Session["loginuser"];
            ViewBag.currentLoginInfo = currentLoginUser;
            return View();
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
                catch (System.Data.Entity.Infrastructure.DbUpdateException)
                {
                    return Content("数据库更新出错");
                }
                catch (System.ObjectDisposedException)
                {
                    return Content("数据上下文连接已过期");
                }
                catch (System.InvalidOperationException)
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


        public ActionResult Blog(int id)
        {
            var currentLoginUser = Session["loginuser"] == null ? null : (User)Session["loginuser"];
            ViewBag.currentLoginInfo = currentLoginUser;
            var model = db.TextLists.FirstOrDefault(m => m.TextID == id);
            return View(model);
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