using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewBeeBlog.ViewModels
{
	public class TopCmtLst
	{
		public string TextTitle { get; set; }
		public int TextID { get; set; }
		public int Num { get; set; }//评论排名
		public int CmtCount { get; set; }
	}
}