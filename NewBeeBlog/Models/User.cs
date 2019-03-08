using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace NewBeeBlog.Models
{
    public class User
    {
        //写入数据库的内容包括用户名和密码，CheckPassword仅用于校验，不写入库
        [Key]
        [MinLength(4)]
        [MaxLength(8)]
        public string Account { get; set; }
        [Required]
        [MinLength(6)]
        [MaxLength(11)]
        public string Password { get; set; }

        [NotMapped]
        [MinLength(6)]
        [MaxLength(11)]
        //TODO:解决验证问题
        //[Compare("Password", ErrorMessage = "密码不一致")]
        public string CheckPassword { get; set; }

        public ICollection<TextList> Textlists { get; set; }
    }
}
