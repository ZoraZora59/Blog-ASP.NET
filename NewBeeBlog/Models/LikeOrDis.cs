using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewBeeBlog.Models
{
    public class LikeOrDis
    {
        //文章ID和用户名共同用于标识唯一的赞或踩
        [Key, Column(Order = 1)]
        [Required]
        public int TextID { get; set; }
        [Key, Column(Order = 2)]
        [MinLength(4)]
        [MaxLength(8)]
        public string Account { get; set; }
        [Required]
        public short LoD { get; set; }

        public ICollection<TextList> Textlists { get; set; }
        public ICollection<User> Users { get; set; }
    }
}