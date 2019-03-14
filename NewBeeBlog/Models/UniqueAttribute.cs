using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace NewBeeBlog.Models
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class UniqueAttribute : ValidationAttribute
    {
        public override Boolean IsValid(Object value)
        {
            //校验数据库是否存在当前Key
            if (value != null)
            {
                return check(value);
            }
            return false;
        }

        private bool check(object o)
        {
            using (NewBeeBlogContext db = new NewBeeBlogContext())
            {
                return db.Categroys.Where(m => m.CategroyName == o.ToString()).Count() <= 0;
            }
        }
    }
}