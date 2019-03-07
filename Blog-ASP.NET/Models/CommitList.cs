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
        public int CommitID { get; set; }

        [Required]
        public int TextID { get; set; }
        [Required]
        public string Account { get; set; }

        [Required]
        [MaxLength(100)]
        public string CommitText { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CommitChangeDate { get; set; }

        public ICollection<TextList> Textlists { get; set; }
    }
}
/*
 *TextID int not null foreign key references TextList(TextID),
  CommitID int identity(10000,1) primary key ,
  Account varchar(8) not null foreign key references Users(Account),
  CommitText nvarchar(100) not null ,
  CommitChangeDate datetime not null default getdate()
 */
