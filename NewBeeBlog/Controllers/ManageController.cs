using NewBeeBlog.App_Code;
using NewBeeBlog.Models;
using NewBeeBlog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace NewBeeBlog.Controllers
{
    public class ManageController : Controller
    {
        
        // GET: Manage
        public ActionResult Index()
        {
            return View();
        }
        // GET: Register
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        // Post: Register
        [HttpPost]
        public ActionResult Register(RegisterUser model)
        {
            if (ModelState.IsValid)
            //判断是否验证通过
            {
                string sessionValidCode = Session["validatecode"]==null?string.Empty: Session["validatecode"].ToString();
                if (!model.Code.Equals(sessionValidCode))
                {
                    return Content("验证码输入错误");
                }
                var user = new User();
                user.Account = model.Account;
                user.Password = md5tool.GetMD5(model.Password);//需要md5加密否则是明文传输
                int res = 0;
                using (NewBeeBlogContext dbContent = new NewBeeBlogContext())
                //防止并发错误

                {
                    dbContent.Users.Add(user);
                    res = dbContent.SaveChanges();
                    //保存数据库
                    //SaveChanges返回的是一个int，大于0则正确直接调转到首页
                }
                if (res > 0)
                {
                    return Redirect("/");
                }
                else
                {
                    return Content("注册失败");
                }
                               
            }
            else
            {
                return Content("验证失败");
            }
        


        }
        // GET: Manage
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginUser model)
        {
            if (ModelState.IsValid)
            {
                string sessionValidCode = Session["validatecode"] == null ? string.Empty : Session["validatecode"].ToString();
                if (!model.Code.Equals(sessionValidCode))
                {
                    return Content("验证码输入错误");
                }
                var user = new User();
                user.Account = model.Account;
                user.Password = md5tool.GetMD5(model.Password);
                int res = 0;
                //根据用户名查找实体
                using (NewBeeBlogContext dbContent = new NewBeeBlogContext())
                {
                    var nameUser = dbContent.Users.FirstOrDefault(m => m.Account == model.Account);
                    if (nameUser == null)
                    {
                        return Content("账号或密码不正确");
                    }
                    else
                    {
                        if (user.Password == nameUser.Password)
                        {
                            Session["loginuser"] = nameUser;//在session中保存用户实体
                            //Response.Write("<script>alert('登录成功!')</script>");
                            return Redirect("/");
                        }
                        else
                        {
                            //Response.Write("<script>alert('登录成功!')</script>");
                            return Content("用户名密码错误");
                        }
                    }
              
                    
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Config()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Config(BlogConfig model)
        {
            new SerializeTool().Serialize<BlogConfig>(model);
            return View();
        }
    }
}