using NewBeeBlog.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NewBeeBlog.ViewModels
{
    public class AddBlog
    {

        [Required]
        [DisplayName("文章标题")]
        [MaxLength(40)]
        public string TextTitle { get; set; }//标题
        
        [Required]
        [DisplayName("文章正文")]
        public string Text { get; set; }//内容
        
        [MaxLength(12)]
        [DisplayName("分类")]
        public string CategoryName { get; set; }//分类
        
    }
}