using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NewBeeBlog.ViewModels
{
    public class ManageText
    {
        [Key]
        [DisplayName("索引号")]
        public int TextID { get; set; }
        [DisplayName("文章标题")]
        public string TextTitle { get; set; }
        [DisplayName("分类")]
        public string CategoryName { get; set; }
        [DisplayName("热度")]
        public int Hot { get; set; }
        [DisplayName("修改时间")]
        public string TextChangeDate { get; set; }

    }
}