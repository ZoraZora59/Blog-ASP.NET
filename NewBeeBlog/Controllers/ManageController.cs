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

        public NewBeeBlogContext db = new NewBeeBlogContext();
        // GET: Manage
        public ActionResult Index()//生成页面时加载Model数据
        {
            ManageMain model = new ManageMain();
            model.UserCount = db.Users.Count();
            model.TextCount = db.TextLists.Count();
            model.CommitCount = db.CommitLists.Count();
            return View(model);
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
                var user = new User
                {
                    Account = model.Account,
                    Password = md5tool.GetMD5(model.Password)//需要md5加密否则是明文传输
                };
                try
                {
                    using (NewBeeBlogContext dbContent = new NewBeeBlogContext())
                    //防止并发错误
                    {
                        dbContent.Users.Add(user);
                        dbContent.SaveChanges();
                        //保存数据库
                    }
                }
                catch(System.Data.Entity.Infrastructure.DbUpdateException)
                {
                    return Content("数据库更新出错");
                }
                catch(System.ObjectDisposedException)
                {
                    return Content("数据上下文连接已过期");
                }
                catch(System.InvalidOperationException)
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
                var user = new User
                {
                    Account = model.Account,
                    Password = md5tool.GetMD5(model.Password)
                };
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
                            Session["loginuser"] = nameUser;
                            return Redirect("/");
                        }
                        else
                        {
                            return Content("账号或密码不正确");
                        }
                    }
              
                    
                }
            }
            return View();
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