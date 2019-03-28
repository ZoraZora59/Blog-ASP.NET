using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace NewBeeBlog.ViewModels
{
    public class ManageMain
    {
        [DisplayName("当前用户数量")]
        public int UserCount { get; set; }
        [DisplayName("当前文章数量")]
        public int TextCount { get; set; }
        [DisplayName("当前评论数量")]
        public int CommitCount { get; set; }
		[DisplayName("当前总点击量")]
		public int HotCount { get; set; }
    }
}