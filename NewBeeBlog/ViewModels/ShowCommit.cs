using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewBeeBlog.ViewModels
{
	public class ShowCommit
	{
		public int Num { get; set; }//第几楼
		public string Name { get; set; }//昵称
		public string Content { get; set; }//内容
		public string Date { get; set; }//发布时间
	}
}