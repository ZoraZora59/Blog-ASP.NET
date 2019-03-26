using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using NewBeeBlog.App_Code;
using NewBeeBlog.Models;
using NewBeeBlog.ViewModels;

namespace NewBeeBlog.DataAlter
{
	#region 分类重命名
	class RenameCategory
	{
		NewBeeBlogContext db = new NewBeeBlogContext();
		public RenameCategory(string NameString)
		{
			string[] NameChange = NameString.Split(new char[] { ',' });
			renameCategoryName(NameChange[0], NameChange[1]);
			db.Dispose();
		}
		private void renameCategoryName(string OldName, string NewName)
		{
			if (NewName == "")
				NewName = null;
			if (OldName == "未分类")//空名字需要专门处理
			{
				while (db.TextLists.FirstOrDefault(c => c.CategoryName == null) != null)
				{
					TextList modelNew = db.TextLists.Where(a => a.CategoryName == null).FirstOrDefault();
					modelNew.CategoryName = NewName;
					db.SaveChanges();
				}
			}
			else
			{
				while (db.TextLists.FirstOrDefault(c => c.CategoryName == OldName) != null)
				{
					TextList modelNew = db.TextLists.Where(a => a.CategoryName == OldName).FirstOrDefault();
					modelNew.CategoryName = NewName;
					db.SaveChanges();
				}
			}
		}
	}
	#endregion

	#region 删除分类
	class RemoveCategory
	{
		NewBeeBlogContext db = new NewBeeBlogContext();
		public RemoveCategory(string Name)
		{
			removeCategory(Name);
			db.Dispose();
		}
		private void removeCategory(string name)
		{
			if (name == "未分类")
				return;//TODO:若分类本来就为空导致无法删除的异常处理
					   //删除分类即分类名置null
			while (db.TextLists.FirstOrDefault(c => c.CategoryName == name) != null)
			{
				TextList modelNew = db.TextLists.Where(a => a.CategoryName == name).FirstOrDefault(); modelNew.CategoryName = null;
				db.SaveChanges();
			}
		}
	}
	#endregion

	#region 删除博文
	class RemoveText
	{
		NewBeeBlogContext db = new NewBeeBlogContext();
		public RemoveText(int tID)
		{
			remove(tID);
		}
		private void remove(int tID)
		{
			int TextID = tID;
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
	}
	#endregion

	#region 用户注册
	class RegistUser
	{
		NewBeeBlogContext db = new NewBeeBlogContext();
		private User user;
		public RegistUser(RegisterUser Ruser)
		{
			flush(Ruser);
			regist();
			db.Dispose();
		}
		private void flush(RegisterUser Ruser)
		{
			this.user = new User
			{
				Account = Ruser.Account,
				Password = md5tool.GetMD5(Ruser.Password),
				Name = Ruser.Name
			};
		}
		private void regist()
		{
			db.Users.Add(user);
			db.SaveChanges();
		}
	}
	#endregion

	#region 删除用户
	class DelUser
	{
		public bool IsSuccess = false;
		private string account;
		NewBeeBlogContext db = new NewBeeBlogContext();
		public DelUser(string Account)
		{
			this.account = Account;
			del();
			db.Dispose();
		}
		private void del()
		{
			var theUser = db.Users.Find(account);
			if (theUser != null)
			{
				while (db.CommitLists.Where(c => c.Account == account).FirstOrDefault() != null)//删除用户的评论
				{
					db.CommitLists.Remove(db.CommitLists.Where(c => c.Account == account).FirstOrDefault());
					db.SaveChanges();
				}
			}
			db.Users.Remove(theUser);
			db.SaveChanges();
			IsSuccess = true;
		}
	}
	#endregion

	#region 更新博文
	class UpdateBlog
	{
		public bool IsSuccess = false;
		private UpdateText blog;
		NewBeeBlogContext db = new NewBeeBlogContext();
		public UpdateBlog(UpdateText Blog)
		{
			this.blog = Blog;
			update();
			db.Dispose();
		}
		private void update()
		{
			if (blog.Id != 0)
			{
				if (db.TextLists.FirstOrDefault(c => c.TextID == blog.Id) != null)
				{
					TextList modelNew = db.TextLists.Where(a => a.TextID == blog.Id).FirstOrDefault();
					modelNew.FirstView = GetFirstView(blog.Text);
					modelNew.TextTitle = blog.Title;
					modelNew.CategoryName = blog.Category;
					modelNew.Text = blog.Text;
					db.SaveChanges();
					IsSuccess = true;
				}
			}
			else
			{
				db.TextLists.Add(new TextList { TextTitle = blog.Title, CategoryName = blog.Category, Text = blog.Text, FirstView = GetFirstView(blog.Text) });
				db.SaveChanges();
				IsSuccess = true;
			}
		}
		private string GetFirstView(string Content)//截取文章预览片段
		{
			Content = Regex.Replace(Content, "<[^>]+>", "");
			Content = Regex.Replace(Content, "&[^;]+;", "");
			if (Content.Length < 105)
				return Content;
			else
				Content = Content.Substring(0, 100);
			return Content;
		}
	}
	#endregion
}