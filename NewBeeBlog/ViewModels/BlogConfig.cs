using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewBeeBlog.ViewModels
{
    public class BlogConfig
    {
        [Required]
        [StringLength(maximumLength: 50)]
        public string Name{ set; get; }

        [Required]
        [StringLength(maximumLength: 200)]
        public string Sign { set; get; }

        [Required]
        [StringLength(maximumLength: 500)]
        public string Note { set; get; }
    }
}