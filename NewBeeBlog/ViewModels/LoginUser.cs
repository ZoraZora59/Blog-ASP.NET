using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewBeeBlog.ViewModels
{
    public class LoginUser
    {
        [Required]
        [StringLength(maximumLength: 16, MinimumLength = 6, ErrorMessage = "用户名必须在6~16位数之间")]
        public string Account { set; get; }

        [Required]
        [StringLength(maximumLength: 16, MinimumLength = 6, ErrorMessage = "密码必须在6~16位数之间")]
        public string Password { set; get; }

        [Required]
        [StringLength(4, ErrorMessage = "注意是4个数字的验证码哦")]
        public string Code { set; get; }
    }
}