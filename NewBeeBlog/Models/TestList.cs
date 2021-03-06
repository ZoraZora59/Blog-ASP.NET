﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace NewBeeBlog.Models
{
    public class TextList
    {
        [Key]
        public int TextID { get; set; }//文章唯一标识

        [Required]
        [DisplayName("文章标题")]
        
        [MaxLength(40)]
        public string TextTitle { get; set; }//标题
        [Required]
        [DisplayName("文章正文")]
        public string Text { get; set; }//内容

		[Required]
		[DisplayName("文章预览")]
		[MaxLength(101)]
		public string FirstView { get; set; }

        [DisplayName("点击量")]
        public int Hot { get; set; }//点击量

        [MaxLength(12)]
        [DisplayName("分类")]
        public string CategoryName { get; set; }//分类
        
		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime TextChangeDate { get; set; }//更新时间
		
        public ICollection<CommitList> CommitLists { get; set; }
    }

}