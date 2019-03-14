using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewBeeBlog.Models
{
    public class Categroy
    {
        [Key]
        public int ID { get; set; }

        [Unique(ErrorMessage ="类型不允许重复")]
        [Required]
        [StringLength(maximumLength: 20)]
        public string CategroyName { get; set; }
        
    }
}