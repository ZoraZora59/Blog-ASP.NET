using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewBeeBlog.ViewModels
{
    public class ManageText
    {
        public int TextID { get; set; }
        public string TextTitle { get; set; }
        public string CategoryName { get; set; }
        public string TextChangeDate { get; set; }
        public string Hot { get; set; }
    }
}