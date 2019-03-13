using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NewBeeBlog.ViewModels
{
    public class ManageUser
    {
        [DisplayName("用户名")]
        public string Account { get; set; }
        [DisplayName("评论数量")]
        public int CommitCount { get; set; }
    }
}