using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewBeeBlog.App_Code;
using NewBeeBlog.Models;
using NewBeeBlog.ViewModels;

namespace NewBeeBlog.DataFlush
{
	// 尝试接口的实现方式
	public interface IDF// Interface of Data Flush
	{
		bool IsSuccess { get; set; }
	}

	//public class DataFlush:IDF
	//{
	//	private static NewBeeBlogContext db = new NewBeeBlogContext();
	//	public List<TextList> DFTextList { get; set; }
	//	public List<CommitList> DFCommitList { get; set; }
	//	public List<User> DFUser { get; set; }
	//}

	#region 获取评论管理界面信息
	class GetCommitList
	{
		NewBeeBlogContext db = new NewBeeBlogContext();
		public List<ShowCommit> SCommits;
		public GetCommitList()
		{
			getAllCommits();
			db.Dispose();
		}
		private void getAllCommits()
		{
			this.SCommits = new List<ShowCommit>();
			var trans = db.CommitLists.Select(m => new { m.CommitID, m.Account, m.TextID, m.CommitText, m.CommitChangeDate }).ToList();
			foreach (var item in trans)
			{
				ShowCommit temp = new ShowCommit
				{
					Account = item.Account,
					Id = item.CommitID,
					Name = db.Users.Where(c => c.Account == item.Account).FirstOrDefault().Name,
					TextId = item.TextID,
					Content = item.CommitText,
					Date = item.CommitChangeDate.ToString()
				};
				this.SCommits.Add(temp);
			}
		}
	}
	#endregion

	#region 获取管理主界面数据
	class GetManage
	{
		NewBeeBlogContext db = new NewBeeBlogContext();
		public ManageMain Mmain;
		public  GetManage()
		{
			getManageIndex();

			db.Dispose();
		}
		private void getManageIndex()
		{
            var blog = from m in db.TextLists
                       select m;
            int totalhot = 0;
            foreach(var item in blog)
            {
                totalhot += item.Hot;
            }
            this.Mmain = new ManageMain
            {
                UserCount = db.Users.Count(),
                TextCount = db.TextLists.Count(),
                CommitCount = db.CommitLists.Count(),
                HotCount = totalhot
            };
		}
	}
	#endregion

	#region 获取文章详情 IN 编辑界面
	class GetTextInUpdate
	{
		NewBeeBlogContext db = new NewBeeBlogContext();
		private int tID;
		public UpdateText Utext;
		public GetTextInUpdate(int tID)
		{
			this.tID = tID;
			getTextContent(tID);
			db.Dispose();
		}
		private void getTextContent(int tID)
		{
			var text = db.TextLists.Find(tID);
			this.Utext = new UpdateText
			{
				Id = text.TextID,
				Title = text.TextTitle,
				Category = text.CategoryName,
				Text = text.Text
			};
		}
	}
	#endregion

	#region 获取分类管理界面信息
	class GetCategoryList
	{
		public List<CategoryList> mod;
		NewBeeBlogContext db = new NewBeeBlogContext();
		public GetCategoryList()
		{
			getCategoryData();
			db.Dispose();
		}
		private void getCategoryData()
		{
			this.mod = new List<CategoryList>();
			List<TextList> temp = new List<TextList>();
			temp = db.TextLists.ToList();
			foreach (var item in temp)
			{
				//这里的categoryItem应当在每次循环时new，以避免重复对同一项进行修改
				var categoryItem = new CategoryList
				{
					CategoryName = item.CategoryName
				};
				//统一命名
				if (categoryItem.CategoryName == null)
				{
					categoryItem.CategoryName = "未分类";
				}
				//若不存在于已生成列表则新添此项
				if (!mod.Exists(T => T.CategoryName == categoryItem.CategoryName))
				{
					categoryItem.CategoryHot += item.Hot;
					categoryItem.TextCount++;
					this.mod.Add(categoryItem);
				}
				//否则对已有项进行更新
				else
				{
					this.mod.Find(CategoryList => CategoryList.CategoryName == categoryItem.CategoryName).CategoryHot += item.Hot;
					this.mod.Find(CategoryList => CategoryList.CategoryName == categoryItem.CategoryName).TextCount++;
				}
			}
			this.mod = mod.OrderByDescending(T => T.CategoryHot).ToList();
		}
	}
	#endregion

	#region 获取分类详情
	class GetCategoryDetail
	{
		public CategoryList CList;//分类详情
		public List<CategoryText> CTList;//分类所属文章表
		NewBeeBlogContext db = new NewBeeBlogContext();
		public GetCategoryDetail(string CategoryName)
		{
			getCategoryDetail(CategoryName);
			db.Dispose();
		}
		private void getCategoryDetail(string Name)
		{
			this.CList = new CategoryList();
			var cname = Name;
			if (cname == "未分类")
				cname = null;
			List<TextList> Tmod = db.TextLists.Where(c => c.CategoryName == cname).ToList();//获取分类下属的文章表
			this.CTList = new List<CategoryText>();
			//将TextList表的数据洗入CategoryText列表
			foreach (var item in Tmod)
			{
				var temp = new CategoryText
				{
					TextTitle = item.TextTitle,
					TextID = item.TextID,
					Hot = item.Hot,
					ChangeTime = item.TextChangeDate
				};
				CList.CategoryHot += item.Hot;
				CTList.Add(temp);
			}
			CList.TextCount = Tmod.Count(c => c.CategoryName == cname);
			if (cname == null)
				CList.CategoryName = "未分类";
		}
	}
	#endregion

	#region 获取文章管理界面信息
	class GetTextList
	{
		public List<ManageText> ManageTexts;
		NewBeeBlogContext db = new NewBeeBlogContext();
		public GetTextList()
		{
			getList();
			db.Dispose();
		}
		private void getList()
		{
			this.ManageTexts = new List<ManageText>();
			var trans = db.TextLists.ToList();
			foreach (var item in trans)
			{
				ManageText temp = new ManageText
				{
					TextID = item.TextID,
					TextTitle = item.TextTitle,
					CategoryName = item.CategoryName,
					TextChangeDate = item.TextChangeDate.ToString(),
					Hot = item.Hot
				};
				ManageTexts.Add(temp);
			}
		}
	}
	#endregion

	#region 获取登录信息
	class Login
	{
		public User LoginData;
		public bool IsLogin=false;
		NewBeeBlogContext db = new NewBeeBlogContext();
		public Login(LoginUser log)
		{
			flush(log);
			checkLog(log);
			db.Dispose();
		}
		private void flush(LoginUser log)
		{
			log.Password = md5tool.GetMD5(log.Password);
		}
		private void checkLog(LoginUser log)
		{
			this.LoginData = db.Users.FirstOrDefault(m => m.Account == log.Account);
			if (LoginData != null)
			{
				if(LoginData.Password==log.Password)
				{
					IsLogin = true;
				}
			}
		}
	}
	#endregion

	#region 获取用户管理界面信息
	class GetUserList
	{
		public List<ManageUser> ManageUsers;
		NewBeeBlogContext db = new NewBeeBlogContext();
		public GetUserList()
		{
			getUserData();
			db.Dispose();
		}
		private void getUserData()
		{
			this.ManageUsers = new List<ManageUser>();
			var trans = db.Users.Select(m => new { m.Account, m.Name }).ToList();
			trans.Remove(trans.Find(a => a.Account == "admin123"));//管理员不能删自己
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
				this.ManageUsers.Add(temp);
			}
		}
	}
	#endregion

	#region 获取文章详情 IN 管理界面
	class GetTextInManageDetail
	{
		public bool IsSuccess;
		public TextList Text;
		NewBeeBlogContext db = new NewBeeBlogContext();
		private int tid;
		public GetTextInManageDetail(int tID)
		{
			this.tid = tID;
			getText();
			db.Dispose();
		}
		private void getText()
		{
			this.Text=db.TextLists.Find(tid);
			if(Text==null)
			{
				IsSuccess = false;
			}
			else
			{
				IsSuccess = true;
			}
		}
	}
	#endregion
}