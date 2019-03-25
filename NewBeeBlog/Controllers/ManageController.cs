using NewBeeBlog.App_Code;
using NewBeeBlog.Models;
using NewBeeBlog.ViewModels;
using NewBeeBlog.DataFlush;
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

        public ActionResult Index()//生成页面时加载Model数据
        {
			return View(new GetManage().Mmain);//可能需要异常处理
        }
        [HttpGet]
        public ActionResult ManageUser()
        {
            return View();
        }
		[HttpGet]
		public ActionResult Update()//文章更新
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
		public ActionResult ManageCommit()
		{
			return View();
		}

        public JsonResult LoadCommit()//加载评论管理界面的数据
        {
            return Json(new GetCommitManage().SCommits);
        }

        [HttpGet]
		public ActionResult RenameCategory()
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
		}//分类更名
		public JsonResult JSRenameCategory()
		{
			//TODO:修改返回值
			var NameString = Request["NameChanging"].ToString();
			DataFlush.DataAlter.RenamaCategory(NameString);
			return Json(0);
		}

		[HttpGet]
        public ActionResult CategoryList()//分类管理
        {
			List<CategoryList> mod = new List<CategoryList>();
			List<TextList> temp = new List<TextList>();
			temp = db.TextLists.ToList();
			foreach(var item in temp)
			{
				//这里的categoryItem应当在每次循环时new，以避免重复对同一项进行修改
				var categoryItem = new CategoryList
				{
					CategoryName = item.CategoryName
				};
				//统一命名
				if (categoryItem.CategoryName==null)
				{
					categoryItem.CategoryName = "未分类";
				}
				//若不存在于已生成列表则新添此项
				if (!mod.Exists(T=>T.CategoryName==categoryItem.CategoryName))
				{
					categoryItem.CategoryHot += item.Hot;
					categoryItem.TextCount ++;
					mod.Add(categoryItem);
				}
				//否则对已有项进行更新
				else
				{
					mod.Find(CategoryList => CategoryList.CategoryName == categoryItem.CategoryName).CategoryHot += item.Hot;
					mod.Find(CategoryList => CategoryList.CategoryName == categoryItem.CategoryName).TextCount ++;
				}
			}
			mod=mod.OrderByDescending(T=>T.CategoryHot).ToList();
			ViewBag.title = "分类管理";
            return View(mod);
        }

		[HttpPost]
		public JsonResult DeleteCategory()//删除指定分类
		{
			string name;
			try
			{
				name = Request["CategoryName"].ToString();
				if (name == "未分类")
					return Json(1);
				//删除分类即分类名置null
				while (db.TextLists.FirstOrDefault(c => c.CategoryName == name) != null)
				{
					TextList modelNew = db.TextLists.Where(a => a.CategoryName == name).FirstOrDefault();
					modelNew.CategoryName = null;
					db.SaveChanges();
				}
			}
			catch (Exception)
			{
				throw;
			}
			return Json(0);
		}

		[HttpGet]
		public ActionResult CategoryDetail()//分类详情
		{
			var mod = new CategoryList();
			try
			{
				var cname = Request["CategoryName"].ToString();
				if (cname == "未分类")
					cname = null;
				List<TextList> Tmod = db.TextLists.Where(c => c.CategoryName == cname).ToList();//获取分类下属的文章表
				List<CategoryText> Cmod = new List<CategoryText>();
				//清洗数据
				foreach (var item in Tmod)
				{
					var temp = new CategoryText
					{
						TextTitle = item.TextTitle,
						TextID = item.TextID,
						Hot = item.Hot,
						ChangeTime = item.TextChangeDate
					};
					Cmod.Add(temp);
				}
				ViewBag.CategoryTextList = Cmod;
				
				mod.TextCount = Tmod.Count(c => c.CategoryName == cname);

				if (cname == null)
					mod.CategoryName = "未分类";

				foreach(var item in Cmod)
				{
					mod.CategoryHot += item.Hot;
				}
			}
			catch (NullReferenceException)//路由地址异常
			{
				return Redirect("/manage/CategoryList");
			}
			catch (Exception)
			{
				return Content("查询分类异常");
			}
			return View(mod);
		}

        [HttpPost]
        public JsonResult DeleteText()//文章删除
        {
            try
            {
                string tID = Request["TextID"].ToString();
                int TextID = int.Parse(tID);
                TextList target = db.TextLists.Find(TextID);
				//删除文章所属的所有评论
				while (db.CommitLists.Where(c => c.TextID == TextID).FirstOrDefault() != null)
				{
					db.CommitLists.Remove(db.CommitLists.Where(c => c.TextID == TextID).FirstOrDefault());
					db.SaveChanges();
				}
				//评论删除后再删除文章，以免后续异常
				db.TextLists.Remove(target);
                db.SaveChanges();
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
        }
        
        public JsonResult LoadTextList()
        {
            List<ManageText> manageTexts = new List<ManageText>();
            var trans = db.TextLists.ToList();
			//清洗数据
            foreach (var item in trans)
            {
				ManageText temp = new ManageText
				{
					TextID = item.TextID,
					TextTitle = item.TextTitle,
					CategoryName = item.CategoryName,
					Date = item.TextChangeDate.ToString(),
					Hot = item.Hot
				};
				manageTexts.Add(temp);
            }
            return Json(manageTexts);
        }

        [HttpGet]
        public ActionResult TextList()//博文管理的列表页
        {
            List<ManageText> ManageTexts = new List<ManageText>();
            List<TextList> trans = db.TextLists.ToList();
            foreach (var t in trans)
            {
				ManageText temp = new ManageText
				{
					TextID = t.TextID,
					TextTitle = t.TextTitle,
					CategoryName = t.CategoryName,
					Hot = t.Hot,
					TextChangeDate = t.TextChangeDate
				};
				ManageTexts.Add(temp);
            }
            return View(ManageTexts);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterUser model)
        {
            if (ModelState.IsValid)
            //判断是否验证通过
            {
                string sessionValidCode = Session["validatecode"] == null ? string.Empty : Session["validatecode"].ToString();
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
                catch (Exception)
                {
                }
            }
            return Redirect("/");
        }

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

        public ActionResult DelUsers(string Account)
        {
            var odlModel = db.Users.Find(Account);
            if (odlModel == null)
            {
                return HttpNotFound();
            }
			while(db.CommitLists.Where(c=>c.Account==Account).FirstOrDefault()!=null)//同时删除用户的评论
			{
				db.CommitLists.Remove(db.CommitLists.Where(c => c.Account == Account).FirstOrDefault());
				db.SaveChanges();
			}
            db.Users.Remove(odlModel);
            db.SaveChanges();
            return Content("删除成功");
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
        public ActionResult Update([Bind(Include = "Id,Title,Category,Text")] UpdateText BlogText)
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
	}
}
