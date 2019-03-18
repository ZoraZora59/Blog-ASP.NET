using NewBeeBlog.App_Code;
using NewBeeBlog.Models;
using NewBeeBlog.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
        public ActionResult ManageUser()//TODO:未完成评论统计
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

		[HttpGet]
		public ActionResult Show()//文章详情
		{
			try
			{
				string jstID = Request["TextID"].ToString();
				if (jstID != null)
				{
					int tid = int.Parse(jstID);
					TextList model = new TextList();
					model=db.TextLists.Find(tid);
					if(model!=null)
						return View(model);
				}
			}
			catch (Exception)
			{
				return Redirect("/Manage/TextList");
				//throw;
			}
			return Redirect("/Manage/TextList");
		}
        //Get:Update
        [HttpGet]
        public ActionResult Update()//文章更新
		{
			try
			{
				string jstID = Request["TextID"].ToString();
				if (jstID != null)
				{
					int tID = int.Parse(jstID);
					var text = new TextList();
					text = GetTextContent(tID);
					ViewBag.title = "文章更新";
					UpdateText Utext = new UpdateText { Id = text.TextID, Title = text.TextTitle, Category = text.CategoryName,Text=text.Text };
					return View(Utext);
				}
			}
			catch(NullReferenceException)
			{
				//TODO：异常处理
			}
			catch (Exception)
			{
				throw;
			}
			ViewBag.Title = "创建文章";
			return View(new UpdateText());
        }
        public TextList GetTextContent(int tID)//根据文章ID查找对应文章
        {
            var text = new TextList();
            text = db.TextLists.Find(tID);//找不到会返回null
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

		[HttpPost]
		//[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult Update([Bind(Include = "Title,Category,Text")] UpdateText BlogText)//增改博文的数据库修改
		{
			if (ModelState.IsValid)
			{
				try
				{
					db.TextLists.Add(new TextList { TextTitle = BlogText.Title, CategoryName = BlogText.Category, Text = BlogText.Text });
					db.SaveChanges();
				}
				catch (Exception)//TODO:添加异常处理信息
				{

					throw;
				}
			}

			return View(BlogText);
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
        //[HttpPost]
        //public ActionResult AddCategroy(AddCategroy model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var categroy = new Categroy();
        //        categroy.CategroyName = model.CategroyName;
        //        try
        //        {
                    
        //            db.Categroys.Add(categroy);
        //            db.SaveChanges();//保存数据库
                    
        //            return Content("添加成功");
        //        }
        //        catch (Exception)
        //        {
        //            return Content("添加失败");
        //            throw;
        //        }
                
        //    }
        //    return View();
        //}
        //[HttpGet]
        //public ActionResult ManageCategroy()
        //{
        //    return View();
        //}
        //public JsonResult LoadCategroy()
        //{
        //    var list =db.Categroys.ToList().Select(m => new { ID = m.ID, CategroyName = m.CategroyName }).ToList();
        //    return Json(list);
        //}

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
		
# region kindeditor操作
		[HttpPost]
		public ActionResult UploadImage()
		{
			string savePath = "/Content/UploadImages/";
			string saveUrl = "/Content/UploadImages/";
			string fileTypes = "gif,jpg,jpeg,png,bmp";
			int maxSize = 1000000;

			Hashtable hash = new Hashtable();

			HttpPostedFileBase file = Request.Files["imgFile"];
			if (file == null)
			{
				hash = new Hashtable();
				hash["error"] = 1;
				hash["message"] = "请选择文件";
				return Json(hash);
			}

			string dirPath = Server.MapPath(savePath);
			if (!Directory.Exists(dirPath))
			{
				hash = new Hashtable();
				hash["error"] = 1;
				hash["message"] = "上传目录不存在";
				return Json(hash);
			}

			string fileName = file.FileName;
			string fileExt = Path.GetExtension(fileName).ToLower();

			ArrayList fileTypeList = ArrayList.Adapter(fileTypes.Split(','));

			if (file.InputStream == null || file.InputStream.Length > maxSize)
			{
				hash = new Hashtable();
				hash["error"] = 1;
				hash["message"] = "上传文件大小超过限制";
				return Json(hash);
			}

			if (string.IsNullOrEmpty(fileExt) || Array.IndexOf(fileTypes.Split(','), fileExt.Substring(1).ToLower()) == -1)
			{
				hash = new Hashtable();
				hash["error"] = 1;
				hash["message"] = "上传文件扩展名是不允许的扩展名";
				return Json(hash);
			}

			string newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
			string filePath = dirPath + newFileName;
			file.SaveAs(filePath);
			string fileUrl = saveUrl + newFileName;

			hash = new Hashtable();
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
				moveupDirPath = Regex.Replace(currentDirPath, @"(.*?)[^\/]+\/$", "$1");
			}

			//排序形式，name or size or type
			String order = Request.QueryString["order"];
			order = String.IsNullOrEmpty(order) ? "" : order.ToLower();

			//不允许使用..移动到上一级目录
			if (Regex.IsMatch(path, @"\.\."))
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
			if (!Directory.Exists(currentPath))
			{
				Response.Write("Directory does not exist.");
				Response.End();
			}

			//遍历目录取得文件信息
			string[] dirList = Directory.GetDirectories(currentPath);
			string[] fileList = Directory.GetFiles(currentPath);

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

			Hashtable result = new Hashtable();
			result["moveup_dir_path"] = moveupDirPath;
			result["current_dir_path"] = currentDirPath;
			result["current_url"] = currentUrl;
			result["total_count"] = dirList.Length + fileList.Length;
			List<Hashtable> dirFileList = new List<Hashtable>();
			result["file_list"] = dirFileList;
			for (int i = 0; i < dirList.Length; i++)
			{
				DirectoryInfo dir = new DirectoryInfo(dirList[i]);
				Hashtable hash = new Hashtable();
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
				FileInfo file = new FileInfo(fileList[i]);
				Hashtable hash = new Hashtable();
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

		public class NameSorter : IComparer
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
				FileInfo xInfo = new FileInfo(x.ToString());
				FileInfo yInfo = new FileInfo(y.ToString());

				return xInfo.FullName.CompareTo(yInfo.FullName);
			}
		}

		public class SizeSorter : IComparer
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
				FileInfo xInfo = new FileInfo(x.ToString());
				FileInfo yInfo = new FileInfo(y.ToString());

				return xInfo.Length.CompareTo(yInfo.Length);
			}
		}

		public class TypeSorter : IComparer
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
				FileInfo xInfo = new FileInfo(x.ToString());
				FileInfo yInfo = new FileInfo(y.ToString());

				return xInfo.Extension.CompareTo(yInfo.Extension);
			}
		}
		#endregion
		
		
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