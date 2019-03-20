using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewBeeBlog.ViewModels
{
	public class CategoryList
	{
		[MaxLength(12)]
		[Display(Name ="类别名称")]
		public string CategoryName { get; set; }
		[Display(Name = "文章数")]
		public int TextCount { get; set; }
		[Display(Name = "总热度")]
		public int CategoryHot { get; set; }
	}
}