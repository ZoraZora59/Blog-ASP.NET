using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewBeeBlog.ViewModels
{
    public class AddCategroy
    {
        [Required]
        [StringLength(maximumLength: 50)]
        public string CategroyName { set; get; }
    }
}