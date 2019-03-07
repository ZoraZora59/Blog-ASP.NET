using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Blog_ASP.NET.Models
{
    public class User
    {
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
        public string CheckPassword { get; set; }

        public ICollection<TextList> Textlists { get; set; }
    }
}