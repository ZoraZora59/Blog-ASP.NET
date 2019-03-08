using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog_ASP.NET.Controllers
{
    public class CalculatorController : Controller
    {
        // GET: Calculator
        public ActionResult Index()
        {
            return View("Calculator");
        }
        public ActionResult Calculate()
        {
            int AddA = Convert.ToInt32(Request.Params["AddA"]);
            int AddB = Convert.ToInt32(Request.Params["AddB"]);
            Blog_ASP.NET.Models.Calculator objCal = new Blog_ASP.NET.Models.Calculator();
            int result = objCal.GetPlus(AddA, AddB);
            ViewData["Result"] = result;
            return View("Calculator");
        }
    }
}