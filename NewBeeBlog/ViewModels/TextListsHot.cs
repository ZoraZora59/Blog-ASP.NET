using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewBeeBlog.ViewModels
{
    public class TextListsHot
    {
        [Key]
        public int TextID { get; set; }//文章唯一标识

        [Required]
        [DisplayName("文章标题")]
        [MaxLength(40)]
        public string TextTitle { get; set; }//标题
        

        [DisplayName("点击量")]
        public int Hot { get; set; }//点击量


        public string CategoryName { get; set; }//分类
        
        public string Datemouth { get; set; }//月份（字符串）
    }
}