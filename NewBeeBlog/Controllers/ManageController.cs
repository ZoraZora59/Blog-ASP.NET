using NewBeeBlog.App_Code;
using NewBeeBlog.Models;
using NewBeeBlog.ViewModels;
using NewBeeBlog.DataFlush;
using NewBeeBlog.DataAlter;
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
	[HandleError]
	public class ManageController : Controller
	{
		private NewBeeBlogContext db = new NewBeeBlogContext();

		public ActionResult Index()//主界面									分层完成
		{
			return View(new GetManage().Mmain);//可能需要异常处理
		}

		[HttpGet]
		public ActionResult ManageUser()            //用户管理				分层完成
		{
			return View();
		}

		[HttpGet]
		public ActionResult Update()//文章更新								分层完成
		{
			try
			{
				string jstID = Request["TextID"].ToString();
				if (jstID != null)
				{
					int tID = int.Parse(jstID);
					ViewBag.title = "文章更新";
					return View(new GetText(tID).Utext);
				}
			}
			catch (NullReferenceException)
			{
			}
			catch (Exception)
			{
				throw;
			}
			ViewBag.Title = "创建文章";
			return View(new UpdateText());
		}

		[HttpGet]
		public ActionResult ManageCommit()      //评论管理					分层完成
		{
			return View();
		}

		public JsonResult LoadCommit()//加载评论管理界面的数据					分层完成
		{
			return Json(new GetCommitManage().SCommits);
		}

		[HttpGet]
		public ActionResult RenameCategory()//分类更名						分层完成
		{
			try
			{
				ViewBag.title = "分类重命名";
				ViewBag.Name = Request["CategoryName"].ToString();
			}
			catch (Exception)
			{
				return Redirect("/manage/CategoryList");
			}
			return View();
		}

		public JsonResult JSRenameCategory()//分类更名的JS实现				分层完成
		{
			//TODO:修改返回值
			var NameString = Request["NameChanging"].ToString();
			new RenameCategory(NameString);
			return Json(0);
		}

		[HttpGet]
		public ActionResult CategoryList()//分类管理							分层完成
		{
			return View(new GetCategory().mod);
		}

		[HttpPost]
		public JsonResult DeleteCategory()//删除指定分类						分层完成
		{
			try
			{
				new RemoveCategory(Request["CategoryName"].ToString());
			}
			catch (Exception)
			{
				throw;
			}
			return Json(0);
		}

		[HttpGet]
		public ActionResult CategoryDetail()//分类详情						分层完成
		{
			try
			{
				var detail = new GetCategoryDetail(Request["CategoryName"].ToString());
				ViewBag.CategoryTextList = detail.CTList;
				return View(detail.CList);
			}
			catch (NullReferenceException)//路由地址异常
			{
				return Redirect("/manage/CategoryList");
			}
			catch (Exception)
			{
				return Redirect("/manage/");
			}
		}

        [HttpPost]
        public JsonResult DeleteText()//文章删除								分层完成
        {
            try
            {
				new RemoveText(int.Parse(Request["TextID"].ToString()));
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            return Json("success");
			//TODO:JS在回复成功后应当删除对应标签
        }
        
        public JsonResult LoadTextList()//文章管理列表的JS实现				分层完成
        {
            return Json(new GetTextList().ManageTexts);
        }

        [HttpGet]
        public ActionResult TextList()//博文管理的列表页						分层完成
        {
            return View(new GetTextList().ManageTexts);
        }

        [HttpGet]
        public ActionResult Register()//注册的页面显示						分层完成
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterUser model)//注册信息提交		分层完成
		{
            if (ModelState.IsValid)
            //判断是否验证通过
            {
                string sessionValidCode = Session["validatecode"] == null ? string.Empty : Session["validatecode"].ToString();
                if (!model.Code.Equals(sessionValidCode))
                {
                    return Content("验证码输入错误,请返回重试");
                }
                try
                {
					new RegistUser(model);
                }
                catch (Exception)
                {
                }
            }
            return Redirect("/");
        }

        [HttpGet]
        public ActionResult Login()//登录的页面显示							分层完成
		{
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginUser model)//登录信息提交				分层完成
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

        public JsonResult LoadCategoryList()
        {
            
            List<TextList> temp = new List<TextList>();
            List<CategoryList> CategoryLists = new List<CategoryList>();
            temp = db.TextLists.ToList();
            foreach (var item in temp)
            {
				var categoryItem = new CategoryList
				{
					CategoryName = item.CategoryName
				};//这里的categoryItem应当在每次循环时new，以避免重复对同一项进行修改
				if (categoryItem.CategoryName == null)
                {
                    categoryItem.CategoryName = "未分类";
                }
                if (!CategoryLists.Exists(T => T.CategoryName == categoryItem.CategoryName))//若不存在于已生成列表则添加进列表
                {
                    categoryItem.CategoryHot += item.Hot;
                    categoryItem.TextCount++;
                    CategoryLists.Add(categoryItem);
                }
                else//否则对指定项进行修改
                {
                    CategoryLists.Find(CategoryList => CategoryList.CategoryName == categoryItem.CategoryName).CategoryHot += item.Hot;
                    CategoryLists.Find(CategoryList => CategoryList.CategoryName == categoryItem.CategoryName).TextCount++;
                }
            }
            return Json(CategoryLists);
        }

        [HttpGet]
        public ActionResult ManageUsers()
        {
            return View();
        }

        public JsonResult LoadUsers()
        {
			List<ManageUser> manageUsers = new List<ManageUser>();
			var trans = db.Users.Select(m => new { m.Account, m.Name }).ToList();
			trans.Remove(trans.Find(a => a.Account == "admin123"));
			foreach (var item in trans)
			{
				ManageUser temp = new ManageUser
				{
					Account = item.Account,
					Name = item.Name,
					CommitCount = 0
				};
				var cmtlist = db.CommitLists.Where(cmt => cmt.Account == temp.Account);
				foreach (var cmt in cmtlist)
				{
					temp.CommitCount++;
				}
				manageUsers.Add(temp);
			}
			return Json(manageUsers);
        }


        [HttpPost]
        public JsonResult DelUsers(string Account)
        {
            var odlModel = db.Users.Find(Account);
            if (odlModel == null)
            {
                return Json(null);
            }
            try
            {
                while (db.CommitLists.Where(c => c.Account == Account).FirstOrDefault() != null)//同时删除用户的评论
                {
                    db.CommitLists.Remove(db.CommitLists.Where(c => c.Account == Account).FirstOrDefault());
                    db.SaveChanges();
                }
                db.Users.Remove(odlModel);
                db.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
            return Json(null);
        }

		
		public string GetFirstView(string Content)//截取文章预览片段
		{
			Content=Regex.Replace(Content, "<[^>]+>", "");
			Content = Regex.Replace(Content, "&[^;]+;", "");
			if (Content.Length < 105)
				return Content;
			else
				Content = Content.Substring(0,100);
			return Content;
		}

		[HttpPost]
        [ValidateInput(false)]
        public ActionResult Update([Bind(Include =
			"Id,Title,Category,Text")] UpdateText BlogText)
        {
            if (ModelState.IsValid)
            {
				if(BlogText.Id!=0)
				{
					if (db.TextLists.FirstOrDefault(c => c.TextID == BlogText.Id) != null)
					{
						//1.先查询要修改的原数据
						TextList modelNew = db.TextLists.Where(a => a.TextID == BlogText.Id).FirstOrDefault();
						modelNew.FirstView = GetFirstView(BlogText.Text);
						//2.设置修改后的值
						modelNew.TextTitle = BlogText.Title;
						modelNew.CategoryName = BlogText.Category;
						modelNew.Text = BlogText.Text;
						db.SaveChanges();
					}
					else
					{
						return Content("更新失败，请确认需要更新的文章是否存在。如需重试请刷新页面......");
					}
				}
				else
				{
					try
					{
						db.TextLists.Add(new TextList { TextTitle = BlogText.Title, CategoryName = BlogText.Category, Text = BlogText.Text, FirstView=GetFirstView(BlogText.Text) });
						db.SaveChanges();
					}
					catch (Exception)//TODO:添加异常处理信息
					{

						throw;
					}
				}
            }

            return Redirect("/manage/textlist");
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
					model = db.TextLists.Find(tid);
					if (model != null)
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

		public ActionResult Upload()
        {
            //文件保存目录路径
            String savePath = "/attached/";
            //文件保存目录URL
            String saveUrl = "/attached/";
			//定义允许上传的文件扩展名
			Hashtable extTable = new Hashtable
			{
				{ "image", "gif,jpg,jpeg,png,bmp" },
				{ "flash", "swf,flv" },
				{ "media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb" },
				{ "file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2" }
			};
			//最大文件大小
			int maxSize = 1000000;
            HttpPostedFileBase imgFile = Request.Files["imgFile"];
            if (imgFile == null)
            {
                return Content("请选择文件。");
            }
            String dirPath = Server.MapPath(savePath);
            if (!Directory.Exists(dirPath))
            {
                return Content("上传目录不存在。");
            }
            String dirName = Request.QueryString["dir"];
            if (String.IsNullOrEmpty(dirName))
            {
                dirName = "image";
            }
            if (!extTable.ContainsKey(dirName))
            {
                return Content("目录名不正确。");
            }
            String fileName = imgFile.FileName;
            String fileExt = Path.GetExtension(fileName).ToLower();
            if (imgFile.InputStream == null || imgFile.InputStream.Length > maxSize)
            {
                return Content("上传文件大小超过限制。");
            }
            if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(((String)extTable[dirName]).Split(','), fileExt.Substring(1).ToLower()) == -1)
            {
                return Content("上传文件扩展名是不允许的扩展名。\n只允许" + ((String)extTable[dirName]) + "格式。");
            }
            //创建文件夹
            dirPath += dirName + "/";
            saveUrl += dirName + "/";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            String ymd = DateTime.Now.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
            dirPath += ymd + "/";
            saveUrl += ymd + "/";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            String newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
            String filePath = dirPath + newFileName;
            imgFile.SaveAs(filePath);
            String fileUrl = saveUrl + newFileName;
			Hashtable hash = new Hashtable
			{
				["error"] = 0,
				["url"] = fileUrl
			};
			return Json(hash);
        }

		#region 全局方法
		protected override void Dispose(bool disposing)//数据连接释放
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

		protected override void HandleUnknownAction(string actionName)//自定义404ERROR
		{
			Response.Redirect("/home");
		}
		#endregion
	}
}
