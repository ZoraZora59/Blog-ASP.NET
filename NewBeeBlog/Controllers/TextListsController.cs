using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NewBeeBlog.Models;

namespace NewBeeBlog.Controllers
{
    public class TextListsController : Controller
    {
        private NewBeeBlogContext db = new NewBeeBlogContext();

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
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TextList model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.TextLists.Add(model);
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    return Content("服务器写入异常");
                    //throw;
                }
                return RedirectToAction("Index");
            }
            return Content("数据提交出错");
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
        public ActionResult Edit([Bind(Include = "TextID,TextTitle,Text,Hot,Attachment,CategoryName,Account,TextChangeDate")] TextList textList)
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

        //以下均为KindEditor控制类

        [HttpPost]
        public ActionResult UploadImage()
        {
            string savePath = "/Content/UploadImages/";
            string saveUrl = "/Content/UploadImages/";
            string fileTypes = "gif,jpg,jpeg,png,bmp";
            int maxSize = 1000000;

            System.Collections.Hashtable hash = new System.Collections.Hashtable();

            HttpPostedFileBase file = Request.Files["imgFile"];
            if (file == null)
            {
                hash = new System.Collections.Hashtable();
                hash["error"] = 1;
                hash["message"] = "请选择文件";
                return Json(hash);
            }

            string dirPath = Server.MapPath(savePath);
            if (!System.IO.Directory.Exists(dirPath))
            {
                hash = new System.Collections.Hashtable();
                hash["error"] = 1;
                hash["message"] = "上传目录不存在";
                return Json(hash);
            }

            string fileName = file.FileName;
            string fileExt = System.IO.Path.GetExtension(fileName).ToLower();

            System.Collections.ArrayList fileTypeList = System.Collections.ArrayList.Adapter(fileTypes.Split(','));

            if (file.InputStream == null || file.InputStream.Length > maxSize)
            {
                hash = new System.Collections.Hashtable();
                hash["error"] = 1;
                hash["message"] = "上传文件大小超过限制";
                return Json(hash);
            }

            if (string.IsNullOrEmpty(fileExt) || Array.IndexOf(fileTypes.Split(','), fileExt.Substring(1).ToLower()) == -1)
            {
                hash = new System.Collections.Hashtable();
                hash["error"] = 1;
                hash["message"] = "上传文件扩展名是不允许的扩展名";
                return Json(hash);
            }

            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", System.Globalization.DateTimeFormatInfo.InvariantInfo) + fileExt;
            string filePath = dirPath + newFileName;
            file.SaveAs(filePath);
            string fileUrl = saveUrl + newFileName;

            hash = new System.Collections.Hashtable();
            hash["error"] = 0;
            hash["url"] = fileUrl;

            return Json(hash, "text/html;charset=UTF-8"); ;

        }

        public ActionResult ProcessRequest()
        {
            //根目录路径，相对路径
            String rootPath = "/Content/UploadImages/";
            //根目录URL，可以指定绝对路径，
            String rootUrl = "/Content/UploadImages/";
            //图片扩展名
            String fileTypes = "gif,jpg,jpeg,png,bmp";

            String currentPath = "";
            String currentUrl = "";
            String currentDirPath = "";
            String moveupDirPath = "";

            //根据path参数，设置各路径和URL
            String path = Request.QueryString["path"];
            path = String.IsNullOrEmpty(path) ? "" : path;
            if (path == "")
            {
                currentPath = Server.MapPath(rootPath);
                currentUrl = rootUrl;
                currentDirPath = "";
                moveupDirPath = "";
            }
            else
            {
                currentPath = Server.MapPath(rootPath) + path;
                currentUrl = rootUrl + path;
                currentDirPath = path;
                moveupDirPath = System.Text.RegularExpressions.Regex.Replace(currentDirPath, @"(.*?)[^\/]+\/$", "$1");
            }

            //排序形式，name or size or type
            String order = Request.QueryString["order"];
            order = String.IsNullOrEmpty(order) ? "" : order.ToLower();

            //不允许使用..移动到上一级目录
            if (System.Text.RegularExpressions.Regex.IsMatch(path, @"\.\."))
            {
                Response.Write("Access is not allowed.");
                Response.End();
            }
            //最后一个字符不是/
            if (path != "" && !path.EndsWith("/"))
            {
                Response.Write("Parameter is not valid.");
                Response.End();
            }
            //目录不存在或不是目录
            if (!System.IO.Directory.Exists(currentPath))
            {
                Response.Write("Directory does not exist.");
                Response.End();
            }

            //遍历目录取得文件信息
            string[] dirList = System.IO.Directory.GetDirectories(currentPath);
            string[] fileList = System.IO.Directory.GetFiles(currentPath);

            switch (order)
            {
                case "size":
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new SizeSorter());
                    break;
                case "type":
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new TypeSorter());
                    break;
                case "name":
                default:
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new NameSorter());
                    break;
            }

            System.Collections.Hashtable result = new System.Collections.Hashtable();
            result["moveup_dir_path"] = moveupDirPath;
            result["current_dir_path"] = currentDirPath;
            result["current_url"] = currentUrl;
            result["total_count"] = dirList.Length + fileList.Length;
            List<System.Collections.Hashtable> dirFileList = new List<System.Collections.Hashtable>();
            result["file_list"] = dirFileList;
            for (int i = 0; i < dirList.Length; i++)
            {
                System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(dirList[i]);
                System.Collections.Hashtable hash = new System.Collections.Hashtable();
                hash["is_dir"] = true;
                hash["has_file"] = (dir.GetFileSystemInfos().Length > 0);
                hash["filesize"] = 0;
                hash["is_photo"] = false;
                hash["filetype"] = "";
                hash["filename"] = dir.Name;
                hash["datetime"] = dir.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                dirFileList.Add(hash);
            }
            for (int i = 0; i < fileList.Length; i++)
            {
                System.IO.FileInfo file = new System.IO.FileInfo(fileList[i]);
                System.Collections.Hashtable hash = new System.Collections.Hashtable();
                hash["is_dir"] = false;
                hash["has_file"] = false;
                hash["filesize"] = file.Length;
                hash["is_photo"] = (Array.IndexOf(fileTypes.Split(','), file.Extension.Substring(1).ToLower()) >= 0);
                hash["filetype"] = file.Extension.Substring(1);
                hash["filename"] = file.Name;
                hash["datetime"] = file.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                dirFileList.Add(hash);
            }
            //Response.AddHeader("Content-Type", "application/json; charset=UTF-8");
            //context.Response.Write(JsonMapper.ToJson(result));
            //context.Response.End();
            return Json(result, "text/html;charset=UTF-8", JsonRequestBehavior.AllowGet);
        }

        public class NameSorter : System.Collections.IComparer
        {
            public int Compare(object x, object y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }
                if (x == null)
                {
                    return -1;
                }
                if (y == null)
                {
                    return 1;
                }
                System.IO.FileInfo xInfo = new System.IO.FileInfo(x.ToString());
                System.IO.FileInfo yInfo = new System.IO.FileInfo(y.ToString());

                return xInfo.FullName.CompareTo(yInfo.FullName);
            }
        }

        public class SizeSorter : System.Collections.IComparer
        {
            public int Compare(object x, object y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }
                if (x == null)
                {
                    return -1;
                }
                if (y == null)
                {
                    return 1;
                }
                System.IO.FileInfo xInfo = new System.IO.FileInfo(x.ToString());
                System.IO.FileInfo yInfo = new System.IO.FileInfo(y.ToString());

                return xInfo.Length.CompareTo(yInfo.Length);
            }
        }

        public class TypeSorter : System.Collections.IComparer
        {
            public int Compare(object x, object y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }
                if (x == null)
                {
                    return -1;
                }
                if (y == null)
                {
                    return 1;
                }
                System.IO.FileInfo xInfo = new System.IO.FileInfo(x.ToString());
                System.IO.FileInfo yInfo = new System.IO.FileInfo(y.ToString());

                return xInfo.Extension.CompareTo(yInfo.Extension);
            }
        }
    }
}
