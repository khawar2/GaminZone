using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GamingWebsite.Controllers
{ [Authorize]
    public class DashboardController : Controller
    {
       
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

       public ActionResult TeamIndex()
        {
            return View();
        }   public ActionResult SoloIndex()
        {
            return View();
        }
    }
}