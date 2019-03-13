using DemoBlog.App_Code;
using NewBeeBlog.App_Code;
using NewBeeBlog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewBeeBlog.Controllers
{
    public class HomeController : Controller
    {


        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            var model = new SerializeTool().DeSerialize<BlogConfig>();
            ViewBag.Config = model;
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Blog()
        {
            return View();
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
    }
}