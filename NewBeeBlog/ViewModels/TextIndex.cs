using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewBeeBlog.ViewModels
{
	public class TextIndex
	{
		public int TextID { get; set; }//文章唯一标识
		
		[DisplayName("文章标题")]
		
		public string TextTitle { get; set; }//标题

		public string FirstView { get; set; }//摘要

		[DisplayName("文章正文")]
		public string Text { get; set; }//内容

		[DisplayName("热度")]
		public int Hot { get; set; }//热度
		
		[DisplayName("分类")]
		public string CategoryName { get; set; }//分类

		[DisplayName("更新时间")]
		public DateTime TextChangeDate { get; set; }//更新时间

		[DisplayName("评论数")]
		public int CommitCount { get; set; }//评论数量

	}
}