using GamingZone.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GamingZone.Controllers
{
    
    public class HomeController : Controller
    {
        [CustomAuthorize("Admin", "User")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Tournaments()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}