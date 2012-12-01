using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc_auction.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";
            ViewBag.Name = "some name";
            
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Welcome()
        {
            return View();
        }

    }
}
