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
		public ActionResult Index()//主界面
		{
			return View(new GetManage().Mmain);//可能需要异常处理
		}

		[HttpGet]
		public ActionResult ManageUser()//用户管理
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
					return View(new GetTextInUpdate(tID).Utext);
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
		public ActionResult ManageCommit()//评论管理
		{
			return View();
		}

		public JsonResult LoadCommit()//加载评论管理界面的数据
		{
			return Json(new GetCommitList().SCommits);
		}

		[HttpGet]
		public ActionResult RenameCategory()//分类更名
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

		public JsonResult JSRenameCategory()//分类更名的JS实现
		{
			//TODO:修改返回值
			var NameString = Request["NameChanging"].ToString();
			new RenameCategory(NameString);
			return Json(0);
		}

		[HttpGet]
		public ActionResult CategoryList()//分类管理
		{
			return View();
		}

		[HttpPost]
		public JsonResult DeleteCategory()//删除指定分类
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
		public ActionResult CategoryDetail()//分类详情
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
        public JsonResult DeleteText()//文章删除
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
        
        public JsonResult LoadTextList()//文章管理列表的JS实现
        {
            return Json(new GetTextList().ManageTexts);
        }

        [HttpGet]
        public ActionResult TextList()//博文管理的列表页
        {
            return View(new GetTextList().ManageTexts);
        }

        [HttpGet]
        public ActionResult Register()//注册的页面显示
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterUser model)//注册信息提交
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
        public ActionResult Login()//登录的页面显示
		{
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginUser model)//登录信息提交
		{
			if (ModelState.IsValid)
			{
				string sessionValidCode = Session["validatecode"] == null ? string.Empty : Session["validatecode"].ToString();
				if (!model.Code.Equals(sessionValidCode))
				{
					return Content("验证码输入错误，请返回上一页重试");
				}
				var isLog = new Login(model);
				if (isLog.IsLogin == false)
				{
					return Content("账号或密码不正确，请返回上一页重试");
				}
				else
				{
					Session["loginuser"] = isLog.LoginData;
					return Redirect("/");
				}
            }
            return View();
        }

        [HttpGet]
        public ActionResult Config()//读取博客配置XML文件
		{
            var model = new SerializeTool().DeSerialize<BlogConfig>();
            return View(model);
        }

        [HttpPost]
        public ActionResult Config(BlogConfig model)//设定博客配置文件
		{
            new SerializeTool().Serialize<BlogConfig>(model);
            return View();
        }

        [HttpGet]
        public ActionResult AddCategroy()//添加分类的页面显示
        {
            return View();
        }

        public JsonResult LoadCategoryList()//加载分类表
        {
            return Json(new GetCategoryList().mod);
        }

        [HttpGet]
        public ActionResult ManageUsers()//用户管理的页面显示
		{
            return View();
        }

        public JsonResult LoadUsers()//加载用户表	
        {
			return Json(new GetUserList().ManageUsers);
        }

        public ActionResult DelUsers(string Account)//删除用户 TODO:JS化
        {
			try
			{
				var IsDel = new DelUser(Account);
				if(IsDel.IsSuccess==false)
				{
					return HttpNotFound();//删除失败的反馈
				}
			}
			catch (Exception)
			{
				//TODO:删除失败的反馈
				throw;
			}
            return Content("删除成功,请刷新页面");//页面更新
        }
		
		[HttpPost]
        [ValidateInput(false)]
        public ActionResult Update([Bind(Include =
			"Id,Title,Category,Text")] UpdateText BlogText)//文章更新提交
        {
            if (ModelState.IsValid)
            {
				try
				{
					var IsUpdate = new UpdateBlog(BlogText);
					if (IsUpdate.IsSuccess == true)//TODO:更新成功处理
					{
						return Redirect("/manage/textlist");
					}
				}
				catch (Exception)
				{
					throw;
				} 
            }
            return Redirect("/manage/textlist");//TODO:更新失败处理
        }

		[HttpGet]
		public ActionResult Show()//文章详情
		{
			try
			{
				var IsGetText = new GetTextInManageDetail(int.Parse(Request["TextID"].ToString()));
				if (IsGetText.IsSuccess == true)
				{
					return View(IsGetText.Text);
				}
			}
			catch (Exception)
			{
				return Redirect("/Manage/TextList");
				//throw;
			}
			return Redirect("/Manage/TextList");
		}

		#region Kindeditor上传
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
		#endregion

		#region 全局方法
		protected override void HandleUnknownAction(string actionName)//自定义404ERROR
		{
			Response.Redirect("/home");
		}
		#endregion
	}
}
