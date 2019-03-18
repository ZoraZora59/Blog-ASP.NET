using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;


namespace System.Web.Mvc
{
    public static class KDEHtmlHelper
    {
        public static MvcHtmlString KindEditorFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, int width, int height)
        {
            //获取属性的名称 tAGBUILDER 
            var exp = expression.Body as MemberExpression;
            string expStr = exp.ToString();//形如 m.name  
            string id = expStr.Substring(expStr.IndexOf(".") + 1);
            string res = string.Format("<textarea name=\"{0}\" style=\"width:\"{2}\" px; height: \"{2}\"px; visibility: hidden; \"></textarea>", id, width, height);

            return MvcHtmlString.Create(res);
        }

        public static NewBeeBlog.Models.NewBeeBlogContext GetDbContext()
        {
            return new NewBeeBlog.Models.NewBeeBlogContext();
        }
    }
}