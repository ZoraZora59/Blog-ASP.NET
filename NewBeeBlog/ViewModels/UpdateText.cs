using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewBeeBlog.ViewModels
{
	public class UpdateText
	{
		[Key]
		public int Id { get; set; }

		[Display(Name ="文章标题")]
		[Required]
		public string Title { get; set; }

		[Required]
		[Display(Name ="文章内容")]
		public string Text { get; set; }

		[Display(Name ="分类")]
		public string Category { get; set; }
        
    }
}