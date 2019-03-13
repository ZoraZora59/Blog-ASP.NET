using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace NewBeeBlog.Models
{
    public class TextList
    {
        [Key]
        public int TextID { get; set; }//文章唯一标识

        [Required]
        [DisplayName("文章标题")]
        [MinLength(4)]
        [MaxLength(40)]
        public string TextTitle { get; set; }//标题
        [Required]
        [DisplayName("文章正文")]
        [MaxLength(20000)]
        public string Text { get; set; }//内容
        [Required]
        [DisplayName("点击量")]
        public int Hot { get; set; }//点击量
        [DisplayName("附件")]
        public string Attachment { get; set; }//附件地址
        [MaxLength(12)]
        [DisplayName("分类")]
        public string CategoryName { get; set; }//分类

        [Required]
        [MinLength(4)]
        [MaxLength(8)]
        public string Account { get; set; }//发布人

        public DateTime TextChangeDate { get; set; }//更新时间

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]

        //关联表
        public ICollection<User> Users { get; set; }
        public ICollection<CommitList> CommitLists { get; set; }
    }
}