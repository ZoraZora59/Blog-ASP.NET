using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewBeeBlog.Models;
using NewBeeBlog.ViewModels;

namespace NewBeeBlog.DataFlush
{
	public class DataAlter:IDF
	{
		private static NewBeeBlogContext db = new NewBeeBlogContext();
		public List<TextList> DFTextList { get; set; }
		public List<CommitList> DFCommitList { get; set; }
		public List<User> DFUser { get; set; }

		#region 分类重命名
		public static void RenamaCategory(string NameString)
		{
			try
			{
				string[] NameChange = NameString.Split(new char[] { ',' });
				renameCategoryName(NameChange[0], NameChange[1]);
			}
			catch
			{
				throw;
			}
		}
		private static void renameCategoryName(string OldName,string NewName)
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
			db.Dispose();
		}
		#endregion
	}
}