using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
}