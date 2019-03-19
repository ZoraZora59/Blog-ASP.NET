using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewBeeBlog.ViewModels
{
	public class CategoryList
	{
		public string CategoryName { get; set; }
		public int TextCount { get; set; }
		public int CategoryHot { get; set; }
	}
}