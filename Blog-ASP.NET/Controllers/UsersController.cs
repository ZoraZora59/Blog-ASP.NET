using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Blog_ASP.NET.Models;
using System.Data.SqlClient;

namespace Blog_ASP.NET.Controllers
{
    public class UsersController : Controller
    {
        private Blog_ASPNETContext db = new Blog_ASPNETContext();
        /*
        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }
        
        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }*/

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string Account,string Password,string CheckPassword)//可直接通过与View页面同名字段进行数据获取
        {
            User user = new User();
            if (CheckPassword == Password)
            {
                ModelState.AddModelError("PwdRepeatError", "成功");//TODO:修改跳转与内部逻辑
            }
            else
            {
                ModelState.AddModelError("PwdRepeatError", "两次输入密码不一致");
            }
            /*if(IsAccountExist(Account))
            {
                Console.WriteLine("AccountExist");
            }*/
            if (ModelState.IsValid)
            {
                try
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    ModelState.AddModelError("DBInputError","数据库写入异常");
                    return View(user);
                    throw;
                }
                
                return RedirectToAction("Details","TextLists");//TODO:修改返回参数 
            }
            return View(user);
        }
        /*
        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Account,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }*/

        /*public bool IsAccountExist(string Ac)
        {
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            if (count > 0)
                return true;
            return false;
        }*/
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
