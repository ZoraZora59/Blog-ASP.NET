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
    public class ManageController : Controller
    {

        private NewBeeBlogContext db = new NewBeeBlogContext();
        // GET: Manage
        // Post:Index
        public ActionResult Index()//生成页面时加载Model数据
        {
            try
            {
                ManageMain model = new ManageMain();
                model.UserCount = db.Users.Count();
                model.TextCount = db.TextLists.Count();
                model.CommitCount = db.CommitLists.Count();
                return View(model);
            }
            catch (Exception)
            {
                //TODO:异常判断
                throw;
            }
        }
        //Get:ManageUser
        [HttpGet]
        public ActionResult ManageUser()
        {
            List<ManageUser> manageUsers = new List<ManageUser>();
            List<User> trans = db.Users.ToList();
            ManageUser temp = new ManageUser();
            foreach (var item in trans)
            {
                temp.Account = item.Account;
                temp.CommitCount = 0;
                var cmtlist = db.CommitLists.Where<CommitList>(cmt => cmt.Account == temp.Account);
                foreach(var cmt in cmtlist)
                {
                    temp.CommitCount++;
                }
                manageUsers.Add(temp);
            }
            return View(manageUsers);
        }
        //Get:Update
        [HttpGet]
        public ActionResult Update()//文章更新
        {
            //try
            //{
            //    string jstID = Request["TextID"].ToString();
            //    if(jstID!=null)
            //    {
            //        int tID = int.Parse(jstID);
            //        var text = new TextList();
            //        text = GetTextContent(tID);
            //        ViewBag.title = "文章更新";
            //        return View(text);
            //    }
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
            //ViewBag.Title = "创建文章";
            return View();
        }
        public TextList GetTextContent(int tID)
        {
            var text = new TextList();
            text = db.TextLists.Find(tID);
            return text;
        }
        //[HttpPost]
        //public ActionResult Update(TextList model)//修改文章
        //{
        //    return View(model);
        //}
        [HttpPost]
        public JsonResult DeleteText()//文章删除
        {
            try
            {

                string tID = Request["TextID"].ToString();
                int TextID = int.Parse(tID);
                TextList target = db.TextLists.Find(TextID);
                db.TextLists.Remove(target);
                db.SaveChanges();
            }
            catch (System.ArgumentNullException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            return Json("success");
            
        }
        // Get:Update
        [HttpGet]
        public ActionResult TextList()//博文管理的列表页
        {
            List<ManageText> ManageTexts = new List<ManageText>();
            List<TextList> trans = db.TextLists.ToList();
            foreach (var t in trans)
            {
                ManageText temp = new ManageText();
                temp.TextID = t.TextID;
                temp.TextTitle = t.TextTitle;
                temp.CategoryName = t.CategoryName;
                temp.Hot = t.Hot;
                temp.TextChangeDate = t.TextChangeDate;
                ManageTexts.Add(temp);
            }
            return View(ManageTexts);
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
                    Password = md5tool.GetMD5(model.Password),//需要md5加密否则是明文传输
                    Name = model.Name
                };
                try
                {
                   db.Users.Add(user);
                   db.SaveChanges();
                   //保存数据库 
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
        [HttpGet]
        public ActionResult Config()
        {
            var model = new SerializeTool().DeSerialize<BlogConfig>();
            return View(model);
        }
        [HttpPost]
        public ActionResult Config(BlogConfig model)
        {
            new SerializeTool().Serialize<BlogConfig>(model);
            return View();

        }
        [HttpGet]
        public ActionResult AddCategroy()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ManageCategroy()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ManageUsers()
        {
            return View();
        }
        public JsonResult LoadUsers()
        {
            var list = db.Users.ToList().Select(m => new { Account = m.Account , Name = m.Name}).ToList();
            foreach (var li in list)
            {
                if (li.Account == "admin123")
                {
                    list.Remove(li);
                    break;
                }
            }
            return Json(list);
        }
        
        public ActionResult DelUsers(string Account)
        {
            
            //var odlModel = db.Users.FirstOrDefault(m => m.Account == Account);
            //Console.WriteLine(Account);
            //if (odlModel!=null)
            //{
            //    DbEntityEntry entry = db.Entry(odlModel);
            //    entry.State = EntityState.Deleted;
            //    int res = db.SaveChanges();
            //    if (res > 0)
            //    {
            //        return Content("删除成功");
            //    }
            //    else
            //    {
            //        return Content("删除失败");
            //    }
            //}
            //return Content("删除失败");
            Console.WriteLine(Account);
            var odlModel = db.Users.Find(Account);
            if(odlModel==null)
            {
                return HttpNotFound();
            }
            db.Users.Remove(odlModel);
            db.SaveChanges();
            return Content("删除成功");

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