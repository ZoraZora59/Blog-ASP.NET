using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Blog_ASP.NET.Models
{
    public class UserLogin
    {
        [Key]
        public int UserID { get; set; }
        public string Account { get; set; }
    }
}