using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewBeeBlog.ViewModels
{
	public class CategoryText
	{
		public int TextID { get; set; }
		public string TextTitle { get; set; }
		public int Hot { get; set; }
		public DateTime ChangeTime { get; set; }
	}
}