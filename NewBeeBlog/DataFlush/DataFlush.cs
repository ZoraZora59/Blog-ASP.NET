using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewBeeBlog.Models;
using NewBeeBlog.ViewModels;

namespace NewBeeBlog.DataFlush
{
	public interface IDF {
		List<TextList> DFTextList { get; set; }
		List<CommitList> DFCommitList { get; set; }
		List<User> DFUser { get; set; }
	}//TODO:设计接口
	public class DataFlush:IDF
	{
		private static NewBeeBlogContext db = new NewBeeBlogContext();
		public List<TextList> DFTextList { get; set; }
		public List<CommitList> DFCommitList { get; set; }
		public List<User> DFUser { get; set; }
	}



    #region 获取评论管理界面信息
    internal class GetCommitManage
    {
        NewBeeBlogContext db = new NewBeeBlogContext();
        public List<ShowCommit> SCommits;
        public GetCommitManage()
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
    internal class GetManage
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
			this.Mmain = new ManageMain
			{
				UserCount = db.Users.Count(),
				TextCount = db.TextLists.Count(),
				CommitCount = db.CommitLists.Count()
			};
		}
	}
	#endregion

	#region 获取文章详情
	internal class GetText
	{
		NewBeeBlogContext db = new NewBeeBlogContext();
		private int tID;
		public UpdateText Utext;
		public GetText(int tID)
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

}