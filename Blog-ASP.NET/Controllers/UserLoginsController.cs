using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Blog_ASP.NET.Models;

namespace Blog_ASP.NET.Controllers
{
    public class UserLoginsController : Controller
    {
        private Blog_ASPNETContext db = new Blog_ASPNETContext();

        // GET: UserLogins
        public ActionResult Index()
        {
            return View(db.UserLogins.ToList());
        }

        // GET: UserLogins/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserLogin userLogin = db.UserLogins.Find(id);
            if (userLogin == null)
            {
                return HttpNotFound();
            }
            return View(userLogin);
        }

        // GET: UserLogins/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserLogins/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,Account")] UserLogin userLogin)
        {
            if (ModelState.IsValid)
            {
                db.UserLogins.Add(userLogin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userLogin);
        }

        // GET: UserLogins/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserLogin userLogin = db.UserLogins.Find(id);
            if (userLogin == null)
            {
                return HttpNotFound();
            }
            return View(userLogin);
        }

        // POST: UserLogins/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,Account")] UserLogin userLogin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userLogin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userLogin);
        }

        // GET: UserLogins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserLogin userLogin = db.UserLogins.Find(id);
            if (userLogin == null)
            {
                return HttpNotFound();
            }
            return View(userLogin);
        }

        // POST: UserLogins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserLogin userLogin = db.UserLogins.Find(id);
            db.UserLogins.Remove(userLogin);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
