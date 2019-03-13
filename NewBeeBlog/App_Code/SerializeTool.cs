using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace NewBeeBlog.App_Code
{
    public class SerializeTool
    {
        public void Serialize<T>(object o )
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            StringWriter sw = new StringWriter();
            xs.Serialize(sw, o);
            string xml = sw.ToString();
            File.WriteAllText(HttpContext.Current.Server.MapPath("~/config/blogconfig.xml"),xml);
        }
    }
}