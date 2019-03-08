using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Blog_ASP.NET.Models
{
    public class TextList
    {
        [Key]
        public int TextID { get; set; }

        [Required]
        [MinLength(4)]
        [MaxLength(40)]
        public string TextTitle { get; set; }
        [Required]
        [MinLength(2)]
        [MaxLength(12)]
        public string Tag { get; set; }
        [Required]
        [MaxLength(8)]
        public string Account { get; set; }
        [Required]
        [MaxLength(4000)]
        public string Text { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime TextChangeDate { get; set; }

        public ICollection<User> Users { get; set; }
        public ICollection<CommitList> CommitLists { get; set; }
    }
}
/*
create table TextList
(
  TextID  int identity(5000,1) primary key ,
  TextTitle nvarchar(40) not null,
  Tag varchar(12) not null,
  Account varchar(8) not null foreign key references Users(Account),
  Text nvarchar(4000) not null,
  TextChangeDate datetime not null default getdate()
)
 */
