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
    public class TextListsController : Controller
    {
        private Blog_ASPNETContext db = new Blog_ASPNETContext();

        // GET: TextLists
        public ActionResult Index()
        {
            return View(db.TextLists.ToList());
        }

        // GET: TextLists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TextList textList = db.TextLists.Find(id);
            if (textList == null)
            {
                return HttpNotFound();
            }
            return View(textList);
        }

        // GET: TextLists/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TextLists/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TextID,TextTitle,Tag,Account,Text,TextChangeDate")] TextList textList)
        {
            if (ModelState.IsValid)
            {
                db.TextLists.Add(textList);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(textList);
        }

        // GET: TextLists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TextList textList = db.TextLists.Find(id);
            if (textList == null)
            {
                return HttpNotFound();
            }
            return View(textList);
        }

        // POST: TextLists/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TextID,TextTitle,Tag,Account,Text,TextChangeDate")] TextList textList)
        {
            if (ModelState.IsValid)
            {
                db.Entry(textList).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(textList);
        }

        // GET: TextLists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TextList textList = db.TextLists.Find(id);
            if (textList == null)
            {
                return HttpNotFound();
            }
            return View(textList);
        }

        // POST: TextLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TextList textList = db.TextLists.Find(id);
            db.TextLists.Remove(textList);
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
