using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Blog_ASP.NET.Models
{
    public class CommitList
    {
        [Key]
        public int CommitID { get; set; }//评论唯一标识

        [Required]
        public int TextID { get; set; }//评论所在文章
        [Required]
        [MaxLength(8)]
        public string Account { get; set; }//发布人

        [Required]
        [MaxLength(100)]
        public string CommitText { get; set; }//内容

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CommitChangeDate { get; set; }//更新日期

        public ICollection<TextList> Textlists { get; set; }
    }
}